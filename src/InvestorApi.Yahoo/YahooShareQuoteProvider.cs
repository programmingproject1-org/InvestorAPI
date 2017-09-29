using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace InvestorApi.Yahoo
{
    /// <summary>
    /// Implements a quote provider using Yahoo.
    /// API Documentation:
    /// http://wern-ancheta.com/blog/2015/04/05/getting-started-with-the-yahoo-finance-api/
    /// </summary>
    internal class YahooShareQuoteProvider : IShareQuoteProvider
    {
        private static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// Returns the current quote for the share with the provided symbol.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the quote for.</param>
        /// <returns>The current quote for the share.</returns>
        public Quote GetQuote(string symbol)
        {
            var quotes = GetQuotes(new[] { symbol });
            if (quotes.ContainsKey(symbol))
            {
                return quotes[symbol];
            }

            return null;
        }

        /// <summary>
        /// Returns the current quote for the shares with the provided symbols.
        /// </summary>
        /// <param name="symbols">The share symbols to retrun the quotes for.</param>
        /// <returns>The crurent quotes for the shares.</returns>
        public IReadOnlyDictionary<string, Quote> GetQuotes(IEnumerable<string> symbols)
        {
            // Download the data as CSV.
            var symbolQuery = string.Join(",", symbols.Distinct().Select(s => s + ".AX"));
            var address = $"http://download.finance.yahoo.com/d/quotes.csv?s={symbolQuery}&f=saa5bb6l1k3ghc1p2";
            var csv = _client.GetStringAsync(address).Result;

            // Parse the data.
            return csv
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => ReadCsvLine(line))
                .Where(quote => quote != null)
                .ToDictionary(quote => quote.Symbol);
        }

        private static Quote ReadCsvLine(string line)
        {
            try
            {
                var values = line.Split(',');

                var symbol = values[0].Substring(1, values[0].Length - 2).Split('.').First();
                var ask = decimal.Parse(values[1]);
                var askSize = long.Parse(values[2]);
                var bid = decimal.Parse(values[3]);
                var bidSize = long.Parse(values[4]);
                var last = decimal.Parse(values[5]);
                var lastSize = long.Parse(values[6]);
                var dayLow = decimal.Parse(values[7]);
                var dayHigh = decimal.Parse(values[8]);
                var change = decimal.Parse(values[9]);
                var changePercent = decimal.Parse(values[10].Substring(1, values[10].Length - 3));

                return new Quote(symbol, ask, askSize, bid, bidSize, last, lastSize, change, changePercent, dayLow, dayHigh);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
