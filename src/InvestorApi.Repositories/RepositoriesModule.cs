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
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            string url = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (string.IsNullOrEmpty(url))
            {
                throw new InvalidOperationException("Environment variable with database connection string not found.");
            }

            string[] urlSegments = url.Split(new[] { '/', ':', '@' }, StringSplitOptions.RemoveEmptyEntries);

            string username = urlSegments[1];
            string password = urlSegments[2];
            string host = urlSegments[3];
            string port = urlSegments[4];
            string database = urlSegments[5];

            string connection = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=True";

            options.UseNpgsql(connection);
        }
    }
}
