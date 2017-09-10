using InvestorApi.ComponentTests.Internal;
using InvestorApi.Contracts;
using InvestorApi.Security;
using Newtonsoft.Json.Linq;
using System;
using Xunit;

namespace InvestorApi.ComponentTests.Steps
{
    internal static class CommonSteps
    {
        public static TestContext GivenImNotAuthenticated(this TestContext context)
        {
            context.SetAccessToken(null);
            return context;
        }

        public static TestContext GivenImInvestor(this TestContext context)
        {
            context.SetAccessToken(JwtIssuer.Issue(Guid.NewGuid(), "Test Investor", "investor@user.com", false));
            return context;
        }

        public static TestContext GivenImAdministrator(this TestContext context)
        {
            context.SetAccessToken(JwtIssuer.Issue(Guid.NewGuid(), "Test Admin", "admin@user.com", true));
            return context;
        }

        public static TestContext ThenStatusCodeShouldBe(this TestContext context, int statusCode)
        {
            Assert.Equal(statusCode, (int)context.LastResponse.StatusCode);
            return context;
        }

        public static TestContext ThenListResultShouldBe(this TestContext context, int returnCount, int totalCount)
        {
            var response = context.ReadResponse<ListResult<JObject>>();

            Assert.Equal(returnCount, response.Items.Count);
            Assert.Equal(totalCount, response.TotalRowCount);

            return context;
        }
    }
}
