using InvestorApi.Contracts;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InvestorApi.Repositories
{
    /// <summary>
    /// The repository to store and retrieve <see cref="Account"/> entities.
    /// </summary>
    internal sealed class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRepository"/> class.
        /// </summary>
        /// <param name="context">The data context.</param>
        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets an account by its unique identifier.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The matching account.</returns>
        public Account GetById(Guid accountId)
        {
            // Get the account and include the positions.
            return _context.Accounts
                .Include(account => account.Positions)
                .Where(account => account.Id == accountId)
                .FirstOrDefault();
        }

        /// <summary>
        /// Lists the account transactions.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="startDate">The start date of the range to return.</param>
        /// <param name="endDate">The end date of the range to return.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>The transactions.</returns>
        public ListResult<Transaction> ListTransactions(Guid accountId, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            // First we have to could the total number of transactions.
            var count = _context.Transactions
                .Where(transaction => transaction.AccountId == accountId)
                .Where(transaction => startDate == null || transaction.TimestampUtc >= startDate)
                .Where(transaction => endDate == null || transaction.TimestampUtc <= endDate)
                .Count();

            // Now we load the transactions for the requested page.
            var items = _context.Transactions
                .Where(transaction => transaction.AccountId == accountId)
                .Where(transaction => startDate == null || transaction.TimestampUtc >= startDate)
                .Where(transaction => endDate == null || transaction.TimestampUtc <= endDate)
                .OrderByDescending(transaction => transaction.TimestampUtc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ListResult<Transaction>(items, pageNumber, pageSize, count);
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account to save.</param>
        public void Save(Account account)
        {
            // Check if the item exists and then either create or update it in the database.
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

        /// <summary>
        /// Resets the specified account.
        /// </summary>
        /// <param name="account">The account to reset.</param>
        public void Reset(Account account)
        {
            // Check if the item exists and then either create or update it in the database.
            var exists = _context.Accounts.AsNoTracking().Any(x => x.Id == account.Id);
            if (exists)
            {
                _context.Transactions.RemoveRange(_context.Transactions.Where(transaction => transaction.AccountId == account.Id));
                _context.Accounts.Update(account);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes the specified account.
        /// </summary>
        /// <param name="account">The account to delete.</param>
        public void Delete(Account account)
        {
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }
    }
}
