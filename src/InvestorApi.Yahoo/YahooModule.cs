using InvestorApi.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace InvestorApi.Yahoo
{
    public static class YahooModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Register the provider in the dependency injection container.
            services.AddSingleton<IShareQuoteProvider>(new YahooQuoteProvider());
        }
    }
}
