using InvestorApi.ComponentTests.Internal;
using InvestorApi.ComponentTests.Steps;
using Xunit;

namespace InvestorApi.ComponentTests
{
    public class GetUser
    {
        [Fact(DisplayName = "Get User - Success")]
        public void Success()
        {
            new TestContext()
                .GivenImAuthenticatedAs("John Smith", "john.smith@getuser.com", "johns-secret")
                .WhenIGetUser()
                .ThenStatusCodeShouldBe(200);
        }

        [Fact(DisplayName = "Get User - User must be authenticated")]
        public void UserMustBeAuthenticated()
        {
            new TestContext()
                .GivenImNotAuthenticated()
                .WhenIGetUser()
                .ThenStatusCodeShouldBe(401);
        }
    }
}
