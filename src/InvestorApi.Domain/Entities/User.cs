using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.Entities
{
    public class User
    {
        private User()
        {
            Accounts = new List<Account>();
            Watchlists = new List<Watchlist>();
        }

        private User(Guid id, string displayName, string email, string hashedPassword, UserLevel level)
            : this()
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

        public ICollection<Account> Accounts { get; private set; }

        public ICollection<Watchlist> Watchlists { get; private set; }

        public static User Register(string displayName, string email, string password)
        {
            return new User(Guid.NewGuid(), displayName, email, password, UserLevel.Investor);
        }

        public void MakeAdministrator()
        {
            Level = UserLevel.Administrator;
        }

        internal UserInfo ToUserInfo()
        {
            var accounts = Accounts.Select(a => a.ToAccountInfo()).ToList();
            var watchlists = Watchlists.Select(a => a.ToWatchlistInfo()).ToList();

            return new UserInfo(Id, Email, DisplayName, Level, accounts, watchlists);
        }
    }
}
