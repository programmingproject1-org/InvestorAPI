using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Domain.Providers;
using InvestorApi.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.Entities
{
    /// <summary>
    /// Encapsulates the domain logic for a user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="User"/> class from being created.
        /// </summary>
        /// <remarks>
        /// This default constructor must not be removed!
        /// It is required by the repository to instantiate new instances of the class.
        /// </remarks>
        private User()
        {
            Accounts = new List<Account>();
            Watchlists = new List<Watchlist>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="email">The email address.</param>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="level">The user level.</param>
        private User(Guid id, string displayName, string email, string hashedPassword, UserLevel level)
            : this()
        {
            Id = id;
            DisplayName = displayName;
            Email = email;
            HashedPassword = hashedPassword;
            Level = level;
        }

        /// <summary>
        /// Gets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the hashed password.
        /// </summary>
        public string HashedPassword { get; private set; }

        /// <summary>
        /// Gets the user level.
        /// </summary>
        public UserLevel Level { get; private set; }

        /// <summary>
        /// Gets the user's accounts.
        /// </summary>
        public ICollection<Account> Accounts { get; private set; }

        /// <summary>
        /// Gets the user's watchlists.
        /// </summary>
        public ICollection<Watchlist> Watchlists { get; private set; }

        /// <summary>
        /// Gets the gravatar URL.
        /// </summary>
        public string GravatarUrl
        {
            get { return GravatarProvider.GetGravatarUrl(Email); }
        }

        /// <summary>
        /// Creates a new user with the supplied information.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="email">The email address.</param>
        /// <param name="password">The hashed password.</param>
        /// <returns>The newly created user.</returns>
        public static User Register(string displayName, string email, string password)
        {
            Validate.NotNullOrWhitespace(displayName, nameof(displayName));
            Validate.NotNullOrWhitespace(email, nameof(email));
            Validate.NotNullOrWhitespace(password, nameof(password));

            return new User(Guid.NewGuid(), displayName, email, password, UserLevel.Investor);
        }

        /// <summary>
        /// Edits the user.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="email">The email address.</param>
        /// <param name="level">The user level.</param>
        public void EditUser(string displayName, string email, UserLevel level)
        {
            Validate.NotNullOrWhitespace(displayName, nameof(displayName));
            Validate.NotNullOrWhitespace(email, nameof(email));

            DisplayName = displayName;
            Email = email;
            Level = level;
        }

        /// <summary>
        /// Exports the state of the entity to a new instance of <see cref="UserInfo"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="UserInfo"/> with the current state of the entity.</returns>
        internal UserInfo ToUserInfo()
        {
            var accounts = Accounts.Select(a => a.ToAccountInfo()).ToList();
            var watchlists = Watchlists.Select(a => a.ToWatchlistInfo()).ToList();

            return new UserInfo(Id, Email, DisplayName, GravatarUrl, Level, accounts, watchlists);
        }
    }
}
