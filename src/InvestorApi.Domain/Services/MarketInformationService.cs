using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Domain.Utilities;

namespace InvestorApi.Domain.Services
{
    internal class MarketInformationService : IMarketInformationService
    {
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
    }
}
