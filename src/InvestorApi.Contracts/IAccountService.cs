using InvestorApi.Contracts.Dtos;
using System;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// A domain service to manage trading accounts.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Gets detailed information about a specific trading accounts.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve the trading account for.</param>
        /// <param name="accountId">The unique identifier of the account to return.</param>
        /// <returns>The trading account details.</returns>
        AccountDetails GetAccountDetails(Guid userId, Guid accountId);

        /// <summary>
        /// Opens the new trading account.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the trading account.</param>
        /// <param name="name">The account name.</param>
        /// <returns>The identifier of the newly created account.</returns>
        Guid CreateAccount(Guid userId, string name);

        /// <summary>
        /// Delete an existing trading account.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the trading account.</param>
        /// <param name="accountId">The unique identifier of the account to delete.</param>
        void DeleteAccount(Guid userId, Guid accountId);
    }
}
