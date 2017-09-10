using InvestorApi.ComponentTests.Internal;
using InvestorApi.ComponentTests.Steps;
using Xunit;

namespace InvestorApi.ComponentTests
{
    public class DeleteUser
    {
        [Fact(DisplayName = "Delete User - Success")]
        public void Success()
        {
            new TestContext()
                .GivenImAuthenticatedAs("John Smith", "one@deleteuser.com", "johns-secret")
                .WhenIDeleteUser()
                .ThenStatusCodeShouldBe(204);
        }

        [Fact(DisplayName = "Delete User - User must exist")]
        public void UserMustExist()
        {
            new TestContext()
                .GivenImAuthenticatedAs("John Smith", "two@deleteuser.com", "johns-secret")
                .WhenIDeleteUser()
                .WhenIDeleteUser()
                .ThenStatusCodeShouldBe(404);
        }

        [Fact(DisplayName = "Delete User - User must be authenticated")]
        public void UserMustBeAuthenticated()
        {
            new TestContext()
                .GivenImNotAuthenticated()
                .WhenIDeleteUser()
                .ThenStatusCodeShouldBe(401);
        }
    }
}
