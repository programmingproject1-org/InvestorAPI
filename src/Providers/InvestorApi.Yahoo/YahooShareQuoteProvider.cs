using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace InvestorApi.Yahoo
{
    /// <summary>
    /// Implements a quote provider using Yahoo.
    /// </summary>
    /// <remarks>
    /// This provider was implemented after the old quote API was decommissioned by Yahoo.
    /// Some values like bid and ask price are generated because there is no more API available that provides these values.
    /// </remarks>
    internal class YahooShareQuoteProvider : IShareQuoteProvider
    {
        private const string BaseUrl = @"https://query1.finance.yahoo.com/v8/finance/chart/{0}?includePrePost=false";

        private static readonly HttpClient _client = new HttpClient();

        private readonly IMarketInfoProvider _marketInfoProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooShareQuoteProvider"/> class.
        /// </summary>
        /// <param name="marketInfoProvider">The market information provider.</param>
        public YahooShareQuoteProvider(IMarketInfoProvider marketInfoProvider)
        {
            if (marketInfoProvider == null)
            {
                throw new ArgumentNullException(nameof(marketInfoProvider));
            }

            _marketInfoProvider = marketInfoProvider;
        }

        /// <summary>
        /// Returns the current quote for the share with the provided symbol.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the quote for.</param>
        /// <returns>The current quote for the share.</returns>
        public Quote GetQuote(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException($"Argument '{nameof(symbol)}' is required.");
            }

            // First we try to get the quote from the intraday prices.
            Quote quote = GetIntradayQuote(symbol);
            if (quote != null)
            {
                return quote;
            }

            // This sometimes fails outside market hours.
            // In that case we fall back to daily prices and use the last day.
            return GetOutsideMarketQuote(symbol);
        }

        /// <summary>
        /// Returns the current quote for the shares with the provided symbols.
        /// </summary>
        /// <param name="symbols">The share symbols to retrun the quotes for.</param>
        /// <returns>The crurent quotes for the shares.</returns>
        public IReadOnlyDictionary<string, Quote> GetQuotes(IEnumerable<string> symbols)
        {
            return symbols.AsParallel()
                .Select(symbol => new { Symbol = symbol, quote = GetQuote(symbol) })
                .Where(i => i.quote != null)
                .ToDictionary(i => i.Symbol, i => i.quote);
        }

        private Quote GetIntradayQuote(string symbol)
        {
            // Format the request URL.
            string requestUrl = string.Format(BaseUrl + "&range=1h", symbol.StartsWith("^") ? symbol : symbol + ".AX");

            // Download the data from the URL and extract the required values.
            var data = DownloadData(requestUrl);

            decimal previousClose = data.Item1["meta"]["previousClose"].Value<decimal>();
            decimal[] lows = data.Item2;
            decimal[] highs = data.Item3;
            decimal[] closes = data.Item4;
            long[] volumes = data.Item5;

            // Now calculate or generate all the required values.
            int decimals = _marketInfoProvider.GetNumberOfDecimals(previousClose);

            decimal last = Math.Round(closes.Last(), decimals);
            long lastVolume = volumes.Where(v => v > 0).DefaultIfEmpty().Last();
            long averageVolume = volumes.Sum() / volumes.Count();

            decimal dayLow = Math.Round(lows.Min(), decimals);
            decimal dayHigh = Math.Round(highs.Max(), decimals);

            decimal change = Math.Round(last - previousClose, decimals);
            decimal changePercent = Math.Round(change / previousClose * 100, 2);

            // Ask and bid prices are no longer provided by Yahoo.
            // We just calculate them using the minimum spread.
            decimal step = _marketInfoProvider.GetMinimumStepSize(previousClose);

            decimal ask = last + step;
            decimal bid = last - step;

            // Now create the quote object an return.
            return new Quote(symbol, ask, bid, last, lastVolume, change, changePercent, dayLow, dayHigh);
        }

        /// <summary>
        /// Returns the current quote for the share with the provided symbol.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the quote for.</param>
        /// <returns>The current quote for the share.</returns>
        private Quote GetOutsideMarketQuote(string symbol)
        {
            // Format the request URL.
            string requestUrl = string.Format(BaseUrl + "&range=1mo&interval=1d", symbol.StartsWith("^") ? symbol : symbol + ".AX");

            // Download the data from the URL and extract the required values.
            var data = DownloadData(requestUrl);

            decimal[] lows = data.Item2;
            decimal[] highs = data.Item3;
            decimal[] closes = data.Item4;
            long[] volumes = data.Item5;

            decimal close = closes.Last();
            decimal previousClose = closes.Reverse().Skip(1).First();

            // Now calculate or generate all the required values.
            int decimals = _marketInfoProvider.GetNumberOfDecimals(previousClose);

            decimal last = Math.Round(close, decimals);
            long lastVolume = volumes.Last();
            long averageVolume = volumes.Sum() / volumes.Count();

            decimal dayLow = Math.Round(lows.Last(), decimals);
            decimal dayHigh = Math.Round(highs.Last(), decimals);

            decimal change = Math.Round(last - previousClose, decimals);
            decimal changePercent = Math.Round(change / previousClose * 100, 2);

            // Ask and bid prices are no longer provided by Yahoo.
            // We just calculate them using the minimum spread.
            decimal step = _marketInfoProvider.GetMinimumStepSize(previousClose);

            decimal ask = last + step;
            decimal bid = last - step;

            // Now create the quote object an return.
            return new Quote(symbol, ask, bid, last, lastVolume, change, changePercent, dayLow, dayHigh);
        }

        private Tuple<JToken, decimal[], decimal[], decimal[], long[]> DownloadData(string requestUrl)
        {
            // Download the JSON document.
            HttpResponseMessage response = _client.GetAsync(requestUrl).Result;
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            // Read and parse the JSON document.
            string responseContent = response.Content.ReadAsStringAsync().Result;

            JObject results = JObject.Parse(responseContent);
            JToken result = results["chart"]["result"][0];

            if (result["timestamp"] == null)
            {
                return null;
            }

            // Read the values from the JSON result.
            return new Tuple<JToken, decimal[], decimal[], decimal[], long[]>(
                result,
                result["indicators"]["quote"][0]["low"].Values<decimal?>().Where(v => v.HasValue).Select(v => v.Value).ToArray(),
                result["indicators"]["quote"][0]["high"].Values<decimal?>().Where(v => v.HasValue).Select(v => v.Value).ToArray(),
                result["indicators"]["quote"][0]["close"].Values<decimal?>().Where(v => v.HasValue).Select(v => v.Value).ToArray(),
                result["indicators"]["quote"][0]["volume"].Values<long?>().Where(v => v.HasValue).Select(v => v.Value).ToArray());
        }
    }
}
