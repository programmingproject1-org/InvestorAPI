using InvestorApi.ComponentTests.Internal;
using InvestorApi.ComponentTests.Steps;
using Xunit;

namespace InvestorApi.ComponentTests
{
    public class ListUsers
    {
        [Fact(DisplayName = "List Users - Success with default")]
        public void Success()
        {
            new TestContext()
                .GivenImAdministrator()
                .GivenUsersExist(5)
                .WhenIListUsers(null, null)
                .ThenStatusCodeShouldBe(200)
                .ThenListResultShouldBe(5, 5);
        }

        [Fact(DisplayName = "List Users - Success non-paged")]
        public void SuccessNonPages()
        {
            new TestContext()
                .GivenImAdministrator()
                .GivenUsersExist(5)
                .WhenIListUsers(1, 100)
                .ThenStatusCodeShouldBe(200)
                .ThenListResultShouldBe(5, 5);
        }

        [Fact(DisplayName = "List Users - Success paged")]
        public void SuccessPaged()
        {
            new TestContext()
                .GivenImAdministrator()
                .GivenUsersExist(5)
                .WhenIListUsers(3, 2)
                .ThenStatusCodeShouldBe(200)
                .ThenListResultShouldBe(1, 5);
        }

        [Fact(DisplayName = "List Users - User must be authenticated")]
        public void UserMustBeAuthenticated()
        {
            new TestContext()
                .GivenImNotAuthenticated()
                .WhenIListUsers(1, 100)
                .ThenStatusCodeShouldBe(401);
        }

        [Fact(DisplayName = "List Users - User must be administrator")]
        public void UserMustBeAdministrator()
        {
            new TestContext()
                .GivenImInvestor()
                .WhenIListUsers(1, 100)
                .ThenStatusCodeShouldBe(403);
        }
    }
}
