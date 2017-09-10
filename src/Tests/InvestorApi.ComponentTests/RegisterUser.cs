using InvestorApi.ComponentTests.Internal;
using InvestorApi.ComponentTests.Steps;
using Xunit;

namespace InvestorApi.ComponentTests
{
    public class RegisterUser
    {
        [Fact(DisplayName = "Register User - Success")]
        public void Success()
        {
            new TestContext()
                .WhenICreateUser("John Smith", "john@hotmail.com", "johns-secret")
                .ThenStatusCodeShouldBe(201);
        }

        [Fact(DisplayName = "Register User - Display name is required")]
        public void DisplayNameIsRequiredError()
        {
            new TestContext()
                .WhenICreateUser(null, "john@gmail.com", "johns-secret")
                .ThenStatusCodeShouldBe(400);
        }

        [Fact(DisplayName = "Register User - Display name must be at least five characters")]
        public void DisplayNameMustBeAtLeastFiveCharactersError()
        {
            new TestContext()
                .WhenICreateUser("John", "john@gmail.com", "johns-secret")
                .ThenStatusCodeShouldBe(400);
        }

        [Fact(DisplayName = "Register User - Email is required")]
        public void EmailIsRequiredError()
        {
            new TestContext()
                .WhenICreateUser("John Smith", null, "johns-secret")
                .ThenStatusCodeShouldBe(400);
        }

        [Fact(DisplayName = "Register User - Email must be valid")]
        public void EmailMustBeValidError()
        {
            new TestContext()
                .WhenICreateUser("John Smith", "john.com", "johns-secret")
                .ThenStatusCodeShouldBe(400);
        }

        [Fact(DisplayName = "Register User - Password is required")]
        public void PasswordIsRequiredError()
        {
            new TestContext()
                .WhenICreateUser("John Smith", "john@gmail.com", null)
                .ThenStatusCodeShouldBe(400);
        }

        [Fact(DisplayName = "Register User - Password must be at least eight characters")]
        public void PasswordMustBeAtLeastEightCharactersError()
        {
            new TestContext()
                .WhenICreateUser("John Smith", "john@gmail.com", "secret")
                .ThenStatusCodeShouldBe(400);
        }
    }
}
