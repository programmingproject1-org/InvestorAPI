using InvestorApi.Contracts;
using InvestorApi.Domain.Providers;
using InvestorApi.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace InvestorApi.Domain
{
    /// <summary>
    /// This class is used by the startup class to register and initialize the components in this library.
    /// </summary>
    public static class DomainModule
    {
        private static Timer _timer;

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

        /// <summary>
        /// Starts the timer to periodically re-calculate the leader boad.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public static void StartTimer(IServiceProvider serviceProvider)
        {
            // Calculate the initial leader board.
            Task.Factory.StartNew(() => RecalculateLeaderBoard(serviceProvider));

            // Setup and start the timer to periodically re-calculate the leader board.
            _timer = new Timer
            {
                Interval = TimeSpan.FromHours(12).TotalMilliseconds,
                AutoReset = true,
                Enabled = true
            };

            _timer.Elapsed += (sender, e) => RecalculateLeaderBoard(serviceProvider);
            _timer.Start();
        }

        /// <summary>
        /// Re-calculates the leader board.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        private static void RecalculateLeaderBoard(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                try
                {
                    var leaderBoardService = scope.ServiceProvider.GetService<ILeaderBoardService>();
                    leaderBoardService.Load();
                }
                catch (Exception ex)
                {
                    var loggerFactory = scope.ServiceProvider.GetService<ILoggerFactory>();
                    loggerFactory.CreateLogger<LeaderBoardService>().LogError(ex, ex.Message);
                }
            }
        }
    }
}
