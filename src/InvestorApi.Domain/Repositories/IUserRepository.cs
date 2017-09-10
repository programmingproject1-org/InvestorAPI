using InvestorApi.Contracts;
using InvestorApi.Domain.Entities;
using System;

namespace InvestorApi.Domain.Repositories
{
    public interface IUserRepository
    {
        User GetById(Guid userId);

        User GetByEmail(string email);

        ListResult<User> ListUsers(int pageNumber, int pageSize);

        void Save(User user);

        void Delete(User user);
    }
}
