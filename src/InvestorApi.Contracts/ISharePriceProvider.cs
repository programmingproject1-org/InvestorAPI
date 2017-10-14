using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// A service to provide market data.
    /// </summary>
    public interface ISharePriceProvider
    {
        /// <summary>
        /// Returns the historical prices for a share within a specified period.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the prices for.</param>
        /// <param name="endTime">The end time of the period.</param>
        /// <param name="range">The date range. Possible values are: 1d, 5d, 1mo, 3mo, 6mo, 1y, 2y, 5y, 10y, ytd, max</param>
        /// <param name="interval">The time interval.</param>
        /// <returns>The historical prices for the share.</returns>
        IReadOnlyCollection<SharePrice> GetHistoricalSharePrices(string symbol, DateTime endTime, string range, string interval);
    }
}
