using InvestorApi.Contracts;
using InvestorApi.Domain.Providers;
using InvestorApi.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvestorApi.Domain
{
    public static class DomainModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Register the domain services in the dependency injection container.
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWatchlistService, WatchlistService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<ILeaderBoardService, LeaderBoardService>();
            services.AddScoped<IMarketInformationService, MarketInformationService>();

            services.AddSingleton<PasswordHashingProvider>();
        }
    }
}
