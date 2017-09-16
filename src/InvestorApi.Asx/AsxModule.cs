using InvestorApi.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace InvestorApi.Asx
{
    public static class AsxModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IShareDetailsProvider>(new AsxShareDetailProvider());
        }
    }
}
