using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InvestorApi.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public User GetById(Guid userId)
        {
            return _context.Users.Find(userId);
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void Save(User user)
        {
            try
            {
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

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
