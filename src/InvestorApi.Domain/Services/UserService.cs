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
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountService _accountService;
        private readonly PasswordHashingProvider _passwordHashingProvider;

        public UserService(
            IUserRepository userRepository,
            IAccountService accountService,
            PasswordHashingProvider passwordHashingProvider)
        {
            _userRepository = userRepository;
            _accountService = accountService;
            _passwordHashingProvider = passwordHashingProvider;
        }

        public UserInfo GetUserInfo(Guid userId)
        {
            var user = _userRepository.GetById(userId);
            return user?.ToUserInfo();
        }

        public ListResult<UserInfo> ListUsers(int pageNumber, int pageSize)
        {
            var result = _userRepository.ListUsers(pageNumber, pageSize);
            return result.Convert(user => user.ToUserInfo());
        }

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

        public Guid RegisterUser(string displayName, string email, string password)
        {
            string hashedPassword = _passwordHashingProvider.ComputeHash(password);
            User user = User.Register(displayName, email, hashedPassword);
            _userRepository.Save(user);

            _accountService.CreateAccount(user.Id, null);

            return user.Id;
        }

        public void MakeUserAdministrator(Guid userId)
        {
            User user = GetUser(userId);
            user.MakeAdministrator();
            _userRepository.Save(user);
        }

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
