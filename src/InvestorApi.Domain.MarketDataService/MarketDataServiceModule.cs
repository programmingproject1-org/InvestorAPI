using InvestorApi.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace InvestorApi.Domain.MarketDataService
{
    public static class MarketDataServiceModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMarketDataService, MarketDataService>();
        }
    }
}
