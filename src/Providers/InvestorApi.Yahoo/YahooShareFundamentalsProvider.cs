using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace InvestorApi.Yahoo
{
    /// <summary>
    /// Implements a quote provider using Yahoo.
    /// </summary>
    internal class YahooShareFundamentalsProvider : IShareFundamentalsProvider
    {
        private const string KeyStatisticsUrl = @"https://query1.finance.yahoo.com/v10/finance/quoteSummary/{0}.AX?formatted=false&lang=en-AU&region=AU&modules=defaultKeyStatistics,financialData,calendarEvents";
        private const string PriceHistoryUrl = @"https://query1.finance.yahoo.com/v8/finance/chart/{0}.AX?range=1y&interval=1d&includePrePost=false";

        private static readonly HttpClient _client = new HttpClient();

        private readonly IMarketInfoProvider _marketInfoProvider;
        private readonly IShareInfoProvider _shareInfoProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooShareFundamentalsProvider"/> class.
        /// </summary>
        /// <param name="marketInfoProvider">The market information provider.</param>
        /// <param name="shareInfoProvider">The share information provider.</param>
        public YahooShareFundamentalsProvider(IMarketInfoProvider marketInfoProvider, IShareInfoProvider shareInfoProvider)
        {
            if (marketInfoProvider == null)
            {
                throw new ArgumentNullException(nameof(marketInfoProvider));
            }

            if (shareInfoProvider == null)
            {
                throw new ArgumentNullException(nameof(shareInfoProvider));
            }

            _marketInfoProvider = marketInfoProvider;
            _shareInfoProvider = shareInfoProvider;
        }

        /// <summary>
        /// Gets the fundamental data for the share with the supplied symbol.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <returns>The share's fundamental data.</returns>
        public ShareFundamentals GetShareFundamentals(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException($"Argument '{nameof(symbol)}' is required.");
            }

            // Get the share information.
            ShareInfo info = _shareInfoProvider.GetShareInfo(symbol);
            if (info == null)
            {
                return null;
            }

            var fundamentals = new ShareFundamentals(info.Symbol, info.Name, info.Industry);

            Task[] tasks = new[]
            {
                GetKeyStatistics(symbol, fundamentals),
                GetPriceHistory(symbol, fundamentals)
            };

            Task.WaitAll(tasks);

            // PE Ratio not provided by Yahoo. We have to calculate it manually.
            if (fundamentals.PreviousClose.HasValue && fundamentals.EarningsShare.HasValue)
            {
                fundamentals.PERatio = Math.Round(fundamentals.PreviousClose.Value / fundamentals.EarningsShare.Value, 2);
            }

            return fundamentals;
        }

        private async Task GetKeyStatistics(string symbol, ShareFundamentals fundamentals)
        {
            // Format the request URL.
            var requestUrl = string.Format(KeyStatisticsUrl, symbol);

            // Download the JSON document.
            HttpResponseMessage response = await _client.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            // Read and parse the JSON document.
            string responseContent = await response.Content.ReadAsStringAsync();
            JObject results = JObject.Parse(responseContent);
            JToken result = results["quoteSummary"]["result"][0];

            fundamentals.MarketCap = ReadValue<long?>(result, "defaultKeyStatistics", "enterpriseValue");
            fundamentals.Change52Weeks = ReadValue<decimal?>(result, "defaultKeyStatistics", "52WeekChange");
            fundamentals.BookValue = ReadValue<decimal?>(result, "defaultKeyStatistics", "bookValue");
            fundamentals.PriceBook = ReadValue<decimal?>(result, "defaultKeyStatistics", "priceToBook");
            fundamentals.TrailingEps = ReadValue<decimal?>(result, "defaultKeyStatistics", "trailingEps");
            fundamentals.ForwardEps = ReadValue<decimal?>(result, "defaultKeyStatistics", "forwardEps");
            fundamentals.EarningsShare = ReadValue<decimal?>(result, "financialData", "revenuePerShare");
            fundamentals.TotalRevenue = ReadValue<long?>(result, "financialData", "totalRevenue");
            fundamentals.EarningsGrowth = ReadValue<decimal?>(result, "financialData", "earningsGrowth");
            fundamentals.RevenueGrowth = ReadValue<decimal?>(result, "financialData", "revenueGrowth");
            fundamentals.TargetHighPrice = ReadValue<decimal?>(result, "financialData", "targetHighPrice");
            fundamentals.TargetLowPrice = ReadValue<decimal?>(result, "financialData", "targetLowPrice");
            fundamentals.TargetMeanPrice = ReadValue<decimal?>(result, "financialData", "targetMeanPrice");
            fundamentals.TargetMedianPrice = ReadValue<decimal?>(result, "financialData", "targetMedianPrice");
            fundamentals.AnalystRecommendation = ReadValue<string>(result, "financialData", "recommendationKey");
            fundamentals.NumberOfAnalystOpinions = ReadValue<int?>(result, "financialData", "numberOfAnalystOpinions");
            fundamentals.ExDividendDate = ReadValue<DateTime?>(result, "calendarEvents", "exDividendDate");
        }

        private async Task GetPriceHistory(string symbol, ShareFundamentals fundamentals)
        {
            // Format the request URL.
            var requestUrl = string.Format(PriceHistoryUrl, symbol);

            // Download the JSON document.
            HttpResponseMessage response = await _client.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            // Read and parse the JSON document.
            string responseContent = await response.Content.ReadAsStringAsync();
            JObject results = JObject.Parse(responseContent);
            JToken result = results["chart"]["result"][0];

            if (result["timestamp"] == null)
            {
                return;
            }

            long[] volume = result["indicators"]["quote"][0]["volume"].Values<long?>().Where(v => v.HasValue).Select(v => v.Value).ToArray();
            if (volume.Any())
            {
                fundamentals.AverageDailyVolume = (long)Math.Round(volume.Average());
            }

            decimal[] close = result["indicators"]["quote"][0]["close"].Values<decimal?>().Where(v => v.HasValue).Select(v => v.Value).Reverse().ToArray();
            if (close.Any())
            {
                // Get the numbers of decimals to round to.
                // Note that the previous close is the first price in the array because it has been reversed.
                int decimals = _marketInfoProvider.GetNumberOfDecimals(close.Last());

                fundamentals.PreviousClose = Math.Round(close.First(), decimals);
                fundamentals.Low52Weeks = Math.Round(close.Min(), decimals);
                fundamentals.High52Weeks = Math.Round(close.Max(), decimals);
                fundamentals.MovingAverage50Days = Math.Round(close.Take(50).Average(), decimals);
                fundamentals.MovingAverage200Days = Math.Round(close.Take(200).Average(), decimals);
            }
        }

        private T ReadValue<T>(JToken token, params string[] keys)
        {
            foreach (string key in keys)
            {
                token = token[key];
                if (token == null)
                {
                    return default(T);
                }
            }

            if (typeof(T) == typeof(DateTime?))
            {
                return (T)(object)DateTimeOffset.FromUnixTimeSeconds(token.Value<long>()).DateTime;
            }

            return token.Value<T>();
        }
    }
}
