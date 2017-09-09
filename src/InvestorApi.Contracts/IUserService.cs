using InvestorApi.Contracts.Dtos;
using System;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// A domain service to manage users (investors, administrators).
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Attempts to authenticate a user by email address and password.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="password">The clear-text password.</param>
        /// <returns>The details of the user if authentication was successful; otherwise false.</returns>
        UserInfo Login(string email, string password);

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="email">The email address.</param>
        /// <param name="password">The clear-text password.</param>
        /// <returns>The identifier of the newly created user.</returns>
        Guid RegisterUser(string displayName, string email, string password);

        /// <summary>
        /// Makes an existing user an administrator.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        void MakeUserAdministrator(Guid userId);

        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        void DeleteUser(Guid userId);
    }
}
