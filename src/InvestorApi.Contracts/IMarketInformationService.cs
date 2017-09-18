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
    }
}
