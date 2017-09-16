using InvestorApi.Contracts;
using InvestorApi.Contracts.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace InvestorApi.ComponentTests.Internal
{
    internal class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env)
            : base(env)
        {
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);

            var service = app.ApplicationServices.GetService(typeof(ISettingService)) as ISettingService;

            service.SaveDefaultAccountSettings(new DefaultAccountSettings { Name = "Default", InitialBalance = 1000000 });

            service.SaveBuyCommissions(new Commissions
            {
                Fixed = new List<CommissionRange>
                {
                    new CommissionRange { Min = 0, Max = 1000000, Value = 50 }
                },
                Percentage = new List<CommissionRange>
                {
                    new CommissionRange { Min = 0, Max = 1000000, Value = 1 }
                }
            });

            service.SaveSellCommissions(new Commissions
            {
                Fixed = new List<CommissionRange>
                {
                    new CommissionRange { Min = 0, Max = 1000000, Value = 50 }
                },
                Percentage = new List<CommissionRange>
                {
                    new CommissionRange { Min = 0, Max = 1000000, Value = 0.25m }
                }
            });
        }

        protected override void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase(databaseName: "Component Tests");
        }
    }
}
