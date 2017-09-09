using System;

namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains information about a user (investor or administrator).
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="email">The email address.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="level">The permission level.</param>
        public UserInfo(Guid id, string email, string displayName, UserLevel level)
        {
            Id = id;
            Email = email;
            DisplayName = displayName;
            Level = level;
        }

        /// <summary>
        /// Gets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets the permission level.
        /// </summary>
        public UserLevel Level { get; private set; }
    }
}
