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
        /// Returnes the hstorical prices for a shere within a specified period.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the prices for.</param>
        /// <param name="startTime">The start time of the period.</param>
        /// <param name="endTime">The end time of the period.</param>
        /// <param name="intervalMinutes">The price interval (in minutes).</param>
        /// <returns>The historical prices for the share.</returns>
        IReadOnlyCollection<SharePrice> GetHistoricalSharePrices(string symbol, DateTime startTime, DateTime endTime, int intervalMinutes);
    }
}
