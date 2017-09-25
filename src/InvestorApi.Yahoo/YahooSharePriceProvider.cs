using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;

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
        /// <summary>
        /// Returnes the hstorical prices for a shere within a specified period.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the prices for.</param>
        /// <param name="startTime">The start time of the period.</param>
        /// <param name="endTime">The end time of the period.</param>
        /// <param name="intervalMinutes">The price interval (in minutes).</param>
        /// <returns>The historical prices for the share.</returns>
        public IReadOnlyCollection<SharePrice> GetHistoricalSharePrices(string symbol, DateTime startTime, DateTime endTime, int intervalMinutes)
        {
            throw new NotImplementedException();
        }
    }
}
