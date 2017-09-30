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
        /// Returns the historical prices for a share within a specified period.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the prices for.</param>
        /// <param name="startTime">The start time of the period.</param>
        /// <param name="endTime">The end time of the period.</param>
        /// <param name="interval">The price interval. Possible values are: 1m, 1h</param>
        /// <param name="range">The date range. Possible values are: 1d, 5d, 1mo, 3mo, 6mo, 1y, 2y, 5y, 10y, ytd, max</param>
        /// <returns>The historical prices for the share.</returns>
        public IReadOnlyCollection<SharePrice> GetHistoricalSharePrices(string symbol, DateTime? startTime, DateTime? endTime, string interval, string range)
        {
            throw new NotImplementedException();
        }
    }
}
