using InvestorApi.Contracts;
using InvestorApi.Domain.Entities;
using System;

namespace InvestorApi.Domain.Repositories
{
    public interface IAccountRepository
    {
        Account GetById(Guid accountId);

        ListResult<Transaction> ListTransactions(Guid accountId, int pageNumber, int pageSize);

        void Save(Account account);

        void Delete(Account account);
    }
}
