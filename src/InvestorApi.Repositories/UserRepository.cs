using InvestorApi.Contracts;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InvestorApi.Repositories
{
    /// <summary>
    /// The repository to store and retrieve <see cref="User"/> entities.
    /// </summary>
    internal sealed class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The data context.</param>
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a user by its unique identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The matching user.</returns>
        public User GetById(Guid userId)
        {
            return _context.Users
                .Include(user => user.Accounts)
                .Include(user => user.Watchlists)
                .Where(user => user.Id == userId)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets a user by its unique email address.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <returns>The matching user.</returns>
        public User GetByEmail(string email)
        {
            return _context.Users
                .Include(user => user.Accounts)
                .Include(user => user.Watchlists)
                .Where(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

        /// <summary>
        /// Lists all users.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>The users.</returns>
        public ListResult<User> ListUsers(int pageNumber, int pageSize)
        {
            // First we have to could the total number of users.
            var count = _context.Users.Count();

            // Now we load the users for the requested page.
            var items = _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(user => user.DisplayName)
                .ToList();

            return new ListResult<User>(items, pageNumber, pageSize, count);
        }

        /// <summary>
        /// Saves the specified user.
        /// </summary>
        /// <param name="user">The user to save.</param>
        public void Save(User user)
        {
            try
            {
                // Check if the user exists and then either create or update it in the database.
                var exists = _context.Users.AsNoTracking().Any(x => x.Id == user.Id);
                if (exists)
                {
                    _context.Users.Update(user);
                }
                else
                {
                    _context.Users.Add(user);
                }

                _context.SaveChanges();
            }
            catch (DbUpdateException ex) when ((ex.InnerException as PostgresException)?.ConstraintName == "Users_Email_Unique")
            {
                throw new ValidationException("There is already a user with this email address.");
            }
        }

        /// <summary>
        /// Deletes the specified user.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
