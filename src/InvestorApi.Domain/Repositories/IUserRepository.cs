using InvestorApi.Contracts;
using InvestorApi.Domain.Entities;
using System;
using System.Collections.Generic;

namespace InvestorApi.Domain.Repositories
{
    /// <summary>
    /// The repository to store and retrieve <see cref="User"/> entities.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets a user by its unique identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The matching user.</returns>
        User GetById(Guid userId);

        /// <summary>
        /// Gets a user by its unique email address.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <returns>The matching user.</returns>
        User GetByEmail(string email);

        /// <summary>
        /// Lists all users.
        /// </summary>
        /// <returns>The users.</returns>
        IReadOnlyCollection<User> ListAllUsersWithAccounts();

        /// <summary>
        /// Lists all users.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>The users.</returns>
        ListResult<User> ListUsers(int pageNumber, int pageSize);

        /// <summary>
        /// Saves the specified user.
        /// </summary>
        /// <param name="user">The user to save.</param>
        void Save(User user);

        /// <summary>
        /// Deletes the specified user.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        void Delete(User user);
    }
}
