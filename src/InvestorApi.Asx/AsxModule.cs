using InvestorApi.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace InvestorApi.Asx
{
    public static class AsxModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Register the provider in the dependency injection container.
            services.AddSingleton<IShareSummaryProvider, AsxShareSummaryProvider>();
        }
    }
}
