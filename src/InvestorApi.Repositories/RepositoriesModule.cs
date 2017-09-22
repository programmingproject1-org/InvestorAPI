using InvestorApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InvestorApi.Repositories
{
    public static class RepositoriesModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Register the repositories in the dependency injection container.
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
        }

        public static void ConfigureInMemoryDbContext(IServiceCollection services)
        {
            // For testing and debugging, we use an in-mempry database. That way we don't have to
            // configure database connection information in development environments.
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("Test Database"));
        }

        public static void ConfigurePostgresDbContext(IServiceCollection services)
        {
            // Heroku injects the database connection URL as an environment variable.
            string url = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (string.IsNullOrEmpty(url))
            {
                throw new InvalidOperationException("Environment variable with database connection string not found.");
            }

            // We need to split the URL and re-format it into a connection string which Entity Framework can understand.
            string[] urlSegments = url.Split(new[] { '/', ':', '@' }, StringSplitOptions.RemoveEmptyEntries);

            string username = urlSegments[1];
            string password = urlSegments[2];
            string host = urlSegments[3];
            string port = urlSegments[4];
            string database = urlSegments[5];

            string connection = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=True";

            // Now initialize Entity Framework with Postgres driver.
            services.AddDbContext<DataContext>(options => options.UseNpgsql(connection));
        }
    }
}
