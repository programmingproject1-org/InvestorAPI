using InvestorApi.Domain.Entities;
using System;

namespace InvestorApi.Domain.Repositories
{
    public interface IUserRepository
    {
        User GetById(Guid userId);

        User GetByEmail(string email);

        void Save(User user);

        void Delete(User user);
    }
}
