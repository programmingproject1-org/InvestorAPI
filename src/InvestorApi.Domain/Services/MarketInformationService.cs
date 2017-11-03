using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Domain.Utilities;

namespace InvestorApi.Domain.Services
{
    /// <summary>
    /// Provides information about the ASX Market.
    /// </summary>
    internal class MarketInformationService : IMarketInformationService
    {
        /// <summary>
        /// Gets the market information.
        /// </summary>
        /// <returns>The market information.</returns>
        public MarketInfo GetMarket()
        {
            var market = new AsxMarket();

            var currentTime = market.GetCurrentTime();
            var isOpen = market.IsMarketOpen();

            var openingTime = market.GetOpeningTime(currentTime);
            if (openingTime < currentTime)
                openingTime = market.GetOpeningTime(openingTime.AddDays(1));

            var closingTime = market.GetClosingTime(currentTime);
            if (closingTime < currentTime)
                closingTime = market.GetClosingTime(closingTime.AddDays(1));

            return new MarketInfo(currentTime, isOpen, openingTime.Subtract(currentTime), closingTime.Subtract(currentTime));
        }

        /// <summary>
        /// Gets the number of decimals to round to.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <returns>The number of decimals for the price.</returns>
        public int GetNumberOfDecimals(decimal price)
        {
            if (price <= 2.00m)
            {
                return 3;
            }

            return 2;

        }

        /// <summary>
        /// Gets the minimum step size for bid and ask prices.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <returns>The minimum step size.</returns>
        public decimal GetMinimumStepSize(decimal price)
        {
            if (price <= 0.10m)
            {
                return 0.001m;
            }

            if (price <= 2.00m)
            {
                return 0.005m;
            }

            return 0.01m;
        }
    }
}
