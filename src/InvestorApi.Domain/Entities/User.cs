using InvestorApi.Contracts;
using System;

namespace InvestorApi.Domain.Entities
{
    public class User
    {
        private User()
        {
            // Required for instantiation by repository.
        }

        private User(Guid id, string displayName, string email, string hashedPassword, UserLevel level)
        {
            Id = id;
            DisplayName = displayName;
            Email = email;
            HashedPassword = hashedPassword;
            Level = level;
        }

        public Guid Id { get; private set; }

        public string DisplayName { get; private set; }

        public string Email { get; private set; }

        public string HashedPassword { get; private set; }

        public UserLevel Level { get; private set; }

        public static User Register(string displayName, string email, string password)
        {
            return new User(Guid.NewGuid(), displayName, email, password, UserLevel.Investor);
        }

        public void MakeAdministrator()
        {
            Level = UserLevel.Administrator;
        }
    }
}
