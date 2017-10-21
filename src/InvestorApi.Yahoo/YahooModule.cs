using InvestorApi.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace InvestorApi.Yahoo
{
    public static class YahooModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Register the provider in the dependency injection container.
            services.AddSingleton<IShareQuoteProvider, YahooShareQuoteProvider>();
            services.AddSingleton<ISharePriceProvider, YahooSharePriceProvider>();
            services.AddSingleton<IShareFundamentalsProvider, YahooShareFundamentalsProvider>();
        }
    }
}
