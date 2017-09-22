using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Contracts.Settings;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Exceptions;
using InvestorApi.Domain.Providers;
using InvestorApi.Domain.Repositories;
using System;

namespace InvestorApi.Domain.Services
{
    /// <summary>
    /// A domain service to manage users (investors, administrators).
    /// </summary>
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountService _accountService;
        private readonly PasswordHashingProvider _passwordHashingProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="accountService">The account service.</param>
        /// <param name="passwordHashingProvider">The password hashing provider.</param>
        public UserService(
            IUserRepository userRepository,
            IAccountService accountService,
            PasswordHashingProvider passwordHashingProvider)
        {
            _userRepository = userRepository;
            _accountService = accountService;
            _passwordHashingProvider = passwordHashingProvider;
        }

        /// <summary>
        /// Gets the user with the supplied id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>The user with the supplied id.</returns>
        public UserInfo GetUserInfo(Guid userId)
        {
            var user = _userRepository.GetById(userId);
            return user?.ToUserInfo();
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="pageNumber">Gets the page number to return.</param>
        /// <param name="pageSize">Gets the page size to apply.</param>
        /// <returns>All users in the system.</returns>
        public ListResult<UserInfo> ListUsers(int pageNumber, int pageSize)
        {
            var result = _userRepository.ListUsers(pageNumber, pageSize);
            return result.Convert(user => user.ToUserInfo());
        }

        /// <summary>
        /// Attempts to authenticate a user by email address and password.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="password">The clear-text password.</param>
        /// <returns>The details of the user if authentication was successful; otherwise false.</returns>
        public UserInfo Login(string email, string password)
        {
            User user = _userRepository.GetByEmail(email);
            if (user == null || user.Level == UserLevel.Friend)
            {
                return null;
            }

            if (!_passwordHashingProvider.VerifyHash(password, user.HashedPassword))
            {
                return null;
            }

            return user.ToUserInfo();
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        /// <param name="email">The email address.</param>
        /// <param name="password">The clear-text password.</param>
        /// <returns>The identifier of the newly created user.</returns>
        public Guid RegisterUser(string displayName, string email, string password)
        {
            // Hash the clear text password.
            string hashedPassword = _passwordHashingProvider.ComputeHash(password);

            // Create the user.
            User user = User.Register(displayName, email, hashedPassword);
            _userRepository.Save(user);

            // Create the user's default account.
            _accountService.CreateAccount(user.Id, null);

            return user.Id;
        }

        /// <summary>
        /// Makes an existing user an administrator.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        public void MakeUserAdministrator(Guid userId)
        {
            User user = GetUser(userId);
            user.MakeAdministrator();
            _userRepository.Save(user);
        }

        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        public void DeleteUser(Guid userId)
        {
            User user = GetUser(userId);
            _userRepository.Delete(user);
        }

        private User GetUser(Guid userId)
        {
            User user = _userRepository.GetById(userId);

            if (user == null || user.Id != userId)
            {
                throw new EntityNotFoundException(nameof(User), userId);
            }

            return user;
        }
    }
}
