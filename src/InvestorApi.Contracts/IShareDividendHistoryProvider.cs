using InvestorApi.Contracts.Dtos;
using System.Collections.Generic;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// A service to provide share dividend history data.
    /// </summary>
    public interface IShareDividendHistoryProvider
    {
        /// <summary>
        /// Gets the dividend history for a share.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="range">The date range. Possible values are: 1y, 2y, 5y, 10y, max</param>
        /// <returns>The dividend history.</returns>
        IReadOnlyCollection<ShareDividend> GetDividendHistory(string symbol, string range);
    }
}
