using InvestorApi.Contracts.Dtos;
using System.Collections.Generic;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// A service to search for and retrieve share summary information.
    /// </summary>
    public interface IShareSummaryProvider
    {
        /// <summary>
        /// Finds shares by the supplied criteria.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="industry">The industry.</param>
        /// <param name="pageNumber">Gets the page number to return.</param>
        /// <param name="pageSize">Gets the page size to apply.</param>
        /// <returns>The list of shares which match the search criteria.</returns>
        ListResult<ShareSummary> FindShares(string searchTerm, string industry, int pageNumber, int pageSize);

        /// <summary>
        /// Returns summary information for the share with the provided symbol.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the details for.</param>
        /// <returns>The share details.</returns>
        ShareSummary GetShareSummary(string symbol);

        /// <summary>
        /// Returns summary information for the shares with the provided symbols.
        /// </summary>
        /// <param name="symbols">The share symbols to retrun the details for.</param>
        /// <returns>The share details.</returns>
        IReadOnlyDictionary<string, ShareSummary> GetShareSummaries(IEnumerable<string> symbols);
    }
}
