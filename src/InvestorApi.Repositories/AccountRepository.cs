using InvestorApi.Contracts;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InvestorApi.Repositories
{
    internal sealed class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public Account GetById(Guid accountId)
        {
            return _context.Accounts
                .Include(account => account.Positions)
                .Where(account => account.Id == accountId)
                .FirstOrDefault();
        }

        public ListResult<Transaction> ListTransactions(Guid accountId, int pageNumber, int pageSize)
        {
            var count = _context.Transactions.Count();

            var items = _context.Transactions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(transaction => transaction.AccountId == accountId)
                .OrderByDescending(transaction => transaction.TimestampUtc)
                .ToList();

            return new ListResult<Transaction>(items, pageNumber, pageSize, count);
        }

        public void Save(Account account)
        {
            var exists = _context.Accounts.AsNoTracking().Any(x => x.Id == account.Id);
            if (exists)
            {
                _context.Accounts.Update(account);
            }
            else
            {
                _context.Accounts.Add(account);
            }

            _context.SaveChanges();
        }

        public void Delete(Account account)
        {
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }
    }
}
