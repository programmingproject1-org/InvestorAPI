using InvestorApi.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace InvestorApi.Yahoo
{
    public static class YahooModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IShareQuoteProvider>(new YahooQuoteProvider());
        }
    }
}
