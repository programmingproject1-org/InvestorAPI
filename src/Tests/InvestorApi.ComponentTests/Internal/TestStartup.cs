using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;

namespace InvestorApi.ComponentTests.Internal
{
    internal class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env)
            : base(env)
        {
        }

        protected override void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        }
    }
}
