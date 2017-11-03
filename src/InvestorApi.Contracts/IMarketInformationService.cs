using InvestorApi.Contracts.Dtos;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// Provides information about the ASX Market.
    /// </summary>
    public interface IMarketInformationService
    {
        /// <summary>
        /// Gets the market information.
        /// </summary>
        /// <returns>The market information.</returns>
        MarketInfo GetMarket();

        /// <summary>
        /// Gets the number of decimals to round to.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <returns>The number of decimals for the price.</returns>
        int GetNumberOfDecimals(decimal price);

        /// <summary>
        /// Gets the minimum step size for bid and ask prices.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <returns>The minimum step size.</returns>
        decimal GetMinimumStepSize(decimal price);
    }
}
