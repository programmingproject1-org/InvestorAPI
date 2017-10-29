using InvestorApi.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InvestorApi.Yahoo
{
    /// <summary>
    /// This class is used by the startup class to register and initialize the components in this library.
    /// </summary>
    public static class YahooModule
    {
        /// <summary>
        /// Regsiteres the services in this library in the applications IOC container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Register the provider in the dependency injection container.
            services.AddSingleton<IShareQuoteProvider, YahooShareQuoteProvider>();
            services.AddSingleton<IShareFundamentalsProvider, YahooShareFundamentalsProvider>();
            services.AddSingleton<ISharePriceHistoryProvider, YahooSharePriceHistoryProvider>();
            services.AddSingleton<IShareDividendHistoryProvider, YahooShareDividendHistoryProvider>();
        }
    }
}
