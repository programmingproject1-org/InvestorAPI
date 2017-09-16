using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Yahoo
{
    internal class YahooQuoteProvider : IShareQuoteProvider
    {
        public Quote GetQuote(string symbol)
        {
            return new Quote(10.90m, 10.80m, 10.80m);
        }

        public IReadOnlyDictionary<string, Quote> GetQuotes(IEnumerable<string> symbols)
        {
            return symbols.ToDictionary(symbol => symbol, symbol => GetQuote(symbol));
        }
    }
}
