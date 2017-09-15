using InvestorApi.ComponentTests.Internal;
using InvestorApi.ComponentTests.Steps;
using Xunit;

namespace InvestorApi.ComponentTests
{
    public class GetToken
    {
        [Fact(DisplayName = "Login - Success")]
        public void Success()
        {
            new TestContext()
                .GivenUserExists("John Smith", "john.smith1@login.com", "johns-secret")
                .WhenIAuthenticateAs("john.smith1@login.com", "johns-secret")
                .ThenStatusCodeShouldBe(200);
        }

        [Fact(DisplayName = "Login - Email must be correct")]
        public void EmailMustbeCorrect()
        {
            new TestContext()
                .GivenUserExists("John Smith", "john.smith2@login.com", "johns-secret")
                .WhenIAuthenticateAs("john.smith2@wrong.com", "johns-secret")
                .ThenStatusCodeShouldBe(401);
        }

        [Fact(DisplayName = "Login - Password must be correct")]
        public void PasswordMustbeCorrect()
        {
            new TestContext()
                .GivenUserExists("John Smith", "john.smith3@login.com", "johns-secret")
                .WhenIAuthenticateAs("john.smith3@login.com", "wrong-secret")
                .ThenStatusCodeShouldBe(401);
        }
    }
}
