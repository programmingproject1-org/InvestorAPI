using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.InMemoryRepositories
{
    internal class UserRepository : IUserRepository
    {
        private static IList<User> _users = new List<User>();

        public User GetById(Guid userId)
        {
            return _users.FirstOrDefault(user => user.Id == userId);
        }

        public User GetByEmail(string email)
        {
            return _users.FirstOrDefault(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void Save(User user)
        {
            var existingUser = GetById(user.Id);
            if (existingUser == null)
            {
                _users.Add(user);
            }
        }

        public void Delete(User user)
        {
            _users.Remove(user);
        }
    }
}
