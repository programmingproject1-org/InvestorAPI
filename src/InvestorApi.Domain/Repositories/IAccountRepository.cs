using InvestorApi.Contracts;
using InvestorApi.Domain.Entities;
using System;

namespace InvestorApi.Domain.Repositories
{
    /// <summary>
    /// The repository to store and retrieve <see cref="Account"/> entities.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Gets an account by its unique identifier.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The matching account.</returns>
        Account GetById(Guid accountId);

        /// <summary>
        /// Lists the account transactions.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>The transactions.</returns>
        ListResult<Transaction> ListTransactions(Guid accountId, int pageNumber, int pageSize);

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account to save.</param>
        void Save(Account account);

        /// <summary>
        /// Deletes the specified account.
        /// </summary>
        /// <param name="account">The account to delete.</param>
        void Delete(Account account);
    }
}
