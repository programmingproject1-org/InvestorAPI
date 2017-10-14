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
    /// A service to provide market data using Yahoo as the data source.
    /// </summary>
    /// <remarks>
    /// Documentation: https://bitbucket.org/SeamusBoyleRMIT/asx-data-source-research/src/master/yahoo-finance.md?fileviewer=file-view-default
    /// </remarks>
    internal sealed class YahooSharePriceProvider : ISharePriceProvider
    {
        private const string BaseUrl = @"https://l1-query.finance.yahoo.com/v8/finance/chart/{0}.AX?range={1}&interval={2}&period2={3}&events=div|split|earn";

        private static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// Returns the historical prices for a share within a specified period.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the prices for.</param>
        /// <param name="endTime">The end time of the period.</param>
        /// <param name="range">The date range. Possible values are: 1d, 5d, 1mo, 3mo, 6mo, 1y, 2y, 5y, 10y, ytd, max</param>
        /// <param name="interval">The time interval.</param>
        /// <returns>The historical prices for the share.</returns>
        public IReadOnlyCollection<SharePrice> GetHistoricalSharePrices(string symbol, DateTime endTime, string range, string interval)
        {
            long period2 = new DateTimeOffset(endTime).ToUnixTimeSeconds();
            string requestUri = string.Format(BaseUrl, symbol, range, interval, period2);
            HttpResponseMessage response = _client.GetAsync(requestUri).Result;
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            string responseContent = response.Content.ReadAsStringAsync().Result;

            JObject results = JObject.Parse(responseContent);
            JToken result = results["chart"]["result"][0];

            if (result["timestamp"] == null)
            {
                return new List<SharePrice>();
            }

            int offset = result["meta"]["gmtoffset"].Value<int>();
            long[] timestamp = result["timestamp"].Values<long>().ToArray();
            decimal?[] low = result["indicators"]["quote"][0]["low"].Values<decimal?>().ToArray();
            decimal?[] high = result["indicators"]["quote"][0]["high"].Values<decimal?>().ToArray();
            decimal?[] open = result["indicators"]["quote"][0]["open"].Values<decimal?>().ToArray();
            decimal?[] close = result["indicators"]["quote"][0]["close"].Values<decimal?>().ToArray();
            long?[] volume = result["indicators"]["quote"][0]["volume"].Values<long?>().ToArray();

            return Enumerable.Range(0, timestamp.Length)
                .Select(i => new SharePrice(
                    ReadTimestamp(timestamp[i], offset),
                    open[i].HasValue ? Math.Round(open[i].Value, 3) : (decimal?)null,
                    high[i].HasValue ? Math.Round(high[i].Value, 3) : (decimal?)null,
                    low[i].HasValue ? Math.Round(low[i].Value, 3) : (decimal?)null,
                    close[i].HasValue ? Math.Round(close[i].Value, 3) : (decimal?)null,
                    volume[i]))
                .ToList();
        }

        private DateTimeOffset ReadTimestamp(long timestamp, int offset)
        {
            var dt = DateTimeOffset.FromUnixTimeSeconds(timestamp).AddSeconds(offset);
            return new DateTimeOffset(dt.DateTime, TimeSpan.FromSeconds(offset));
        }
    }
}
