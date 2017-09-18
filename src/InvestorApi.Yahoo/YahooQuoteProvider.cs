using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace InvestorApi.Yahoo
{
    internal class YahooQuoteProvider : IShareQuoteProvider
    {
        private static readonly HttpClient _client = new HttpClient();

        public Quote GetQuote(string symbol)
        {
            var quotes = GetQuotes(new[] { symbol });
            if (quotes.ContainsKey(symbol))
            {
                return quotes[symbol];
            }

            return null;
        }

        public IReadOnlyDictionary<string, Quote> GetQuotes(IEnumerable<string> symbols)
        {
            var symbolQuery = string.Join(",", symbols.Select(s => s + ".AX"));
            var address = $"http://download.finance.yahoo.com/d/quotes.csv?s={symbolQuery}&f=saa5bb6l1k3gh";

            var csv = _client.GetStringAsync(address).Result;
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
                var askSize = int.Parse(values[2]);
                var bid = decimal.Parse(values[3]);
                var bidSize = int.Parse(values[4]);
                var last = decimal.Parse(values[5]);
                var lastSize = int.Parse(values[6]);
                var dayLow = decimal.Parse(values[7]);
                var dayHigh = decimal.Parse(values[8]);

                return new Quote(symbol, ask, askSize, bid, bidSize, last, lastSize, dayLow, dayHigh);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
