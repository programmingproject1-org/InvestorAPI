using InvestorApi.Contracts.Dtos;
using System.Collections.Generic;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// A service to provide market data.
    /// </summary>
    public interface IShareQuoteProvider
    {
        /// <summary>
        /// Returns the current quote for the share with the provided symbol.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the quote for.</param>
        /// <returns>The current quote for the share.</returns>
        Quote GetQuote(string symbol);

        /// <summary>
        /// Returns the current quote for the shares with the provided symbols.
        /// </summary>
        /// <param name="symbols">The share symbols to retrun the quotes for.</param>
        /// <returns>The current quotes for the shares.</returns>
        IReadOnlyDictionary<string, Quote> GetQuotes(IEnumerable<string> symbols);
    }
}
