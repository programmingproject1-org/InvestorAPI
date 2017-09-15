using InvestorApi.ComponentTests.Internal;
using InvestorApi.Models;

namespace InvestorApi.ComponentTests.Steps
{
    internal static class UserSteps
    {
        public static TestContext GivenImAuthenticatedAs(this TestContext context, string displayName, string email, string password)
        {
            context.WhenICreateUser(displayName, email, password);
            context.Post("/token", new Login { Email = email, Password = password });
            context.SetAccessToken(context.ReadResponse<LoginResponse>().AccessToken);
            return context;
        }

        public static TestContext GivenUserExists(this TestContext context, string displayName, string email, string password)
        {
            return context.WhenICreateUser(displayName, email, password);
        }

        public static TestContext GivenUsersExist(this TestContext context, int count)
        {
            for (int i = 0; i < count; i++)
            {
                context.WhenICreateUser("Test User", "user" + i + "@host.com", "test password");
            }

            return context;
        }

        public static TestContext WhenIAuthenticateAs(this TestContext context, string email, string password)
        {
            context.Post("/token", new Login { Email = email, Password = password });
            return context;
        }

        public static TestContext WhenICreateUser(this TestContext context, string displayName, string email, string password)
        {
            var request = new CreateUser
            {
                DisplayName = displayName,
                Email = email,
                Password = password
            };

            context.Post("/users", request);
            return context;
        }

        public static TestContext WhenIDeleteUser(this TestContext context)
        {
            context.Delete("/users");
            return context;
        }

        public static TestContext WhenIGetUser(this TestContext context)
        {
            context.Get("/users");
            return context;
        }

        public static TestContext WhenIListUsers(this TestContext context, int? pageNumber, int? pageSize)
        {
            string resourceUrl = "/admin/users";

            if (pageNumber.HasValue)
            {
                resourceUrl += $"?pageNumber={pageNumber}";
            }

            if (pageSize.HasValue)
            {
                resourceUrl += $"&pageSize={pageSize}";
            }

            context.Get(resourceUrl);
            return context;
        }
    }
}
