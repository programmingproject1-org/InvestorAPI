using InvestorApi.Contracts.Dtos;
using System.Collections.Generic;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// A service to provide market data.
    /// </summary>
    public interface IShareDetailsProvider
    {
        /// <summary>
        /// Returned detailed information for the share with the provided symbol.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the details for.</param>
        /// <returns>The share details.</returns>
        ShareDetails GetShareDetails(string symbol);

        /// <summary>
        /// Returned detailed information for the shares with the provided symbols.
        /// </summary>
        /// <param name="symbols">The share symbols to retrun the details for.</param>
        /// <returns>The share details.</returns>
        IReadOnlyDictionary<string, ShareDetails> GetShareDetails(IEnumerable<string> symbols);

        /// <summary>
        /// Finds shares by the supplied criteria.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="industry">The industry.</param>
        /// <param name="pageNumber">Gets the page number to return.</param>
        /// <param name="pageSize">Gets the page size to apply.</param>
        /// <returns>The list of shares which match the search criteria.</returns>
        ListResult<ShareDetails> FindShareDetails(string searchTerm, string industry, int pageNumber, int pageSize);
    }
}
