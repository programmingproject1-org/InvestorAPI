using InvestorApi.ComponentTests.Internal;
using InvestorApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace InvestorApi.ComponentTests
{
    public class RegisterUserTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public RegisterUserTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task Success()
        {
            await ExecuteTest("John Smith", "john@hotmail.com", "johns-secret", 201);
        }

        [Fact]
        public async Task DisplayNameIsRequiredError()
        {
            await ExecuteTest(null, "john@gmail.com", "johns-secret", 400);
        }

        [Fact]
        public async Task DisplayNameMustBeAtLeastFiveCharactersError()
        {
            await ExecuteTest("John", "john@gmail.com", "johns-secret", 400);
        }

        [Fact]
        public async Task EmailIsRequiredError()
        {
            await ExecuteTest("John Smith", null, "johns-secret", 400);
        }

        [Fact]
        public async Task EmailMustBeValidError()
        {
            await ExecuteTest("John Smith", "john.com", "johns-secret", 400);
        }

        [Fact]
        public async Task PasswordIsRequiredError()
        {
            await ExecuteTest("John Smith", "john@gmail.com", null, 400);
        }

        [Fact]
        public async Task PasswordMustBeAtLeastSixCharactersError()
        {
            await ExecuteTest("John Smith", "john@gmail.com", "sec", 400);
        }

        private async Task ExecuteTest(string displayName, string email, string password, int expectedStatusCode)
        {
            var request = new CreateUser
            {
                DisplayName = displayName,
                Email = email,
                Password = password
            };

            var response = await _client.PostAsync("/api/1.0/users", new JsonContent(request));

            Assert.Equal(expectedStatusCode, (int)response.StatusCode);
        }
    }
}
