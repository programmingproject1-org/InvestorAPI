using InvestorApi.Contracts;
using InvestorApi.Domain.Providers;
using InvestorApi.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InvestorApi.Domain
{
    /// <summary>
    /// This class is used by the startup class to register and initialize the components in this library.
    /// </summary>
    public static class DomainModule
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

            // Register the domain services in the dependency injection container.
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWatchlistService, WatchlistService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<ILeaderBoardService, LeaderBoardService>();

            services.AddSingleton<PasswordHashingProvider>();
        }
    }
}
