using InvestorApi.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace InvestorApi.Domain.InMemoryRepositories
{
    public static class InMemoryRepositoriesModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
