using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;

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
        /// Lists the account's transactions.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve the trading account for.</param>
        /// <param name="accountId">The unique identifier of the account to return.</param>
        /// <param name="startDate">The start date of the range to return.</param>
        /// <param name="endDate">The end date of the range to return.</param>
        /// <param name="pageNumber">Gets the page number to return.</param>
        /// <param name="pageSize">Gets the page size to apply.</param>
        /// <returns>The transactions.</returns>
        ListResult<TransactionInfo> ListTransactions(Guid userId, Guid accountId, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);

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

        /// <summary>
        /// Resets a trading account to its starting state.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the trading account.</param>
        /// <param name="accountId">The unique identifier of the account to reset.</param>
        void ResetAccount(Guid userId, Guid accountId);

        /// <summary>
        /// Buys the supplied quantity of shares at the current market price.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the trading account.</param>
        /// <param name="accountId">The unique identifier of the account.</param>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="quantity">The quantity to buy.</param>
        /// <param name="nonce">The nonce value required to detect suplicate orders.</param>
        void BuySharesAtMarketPrice(Guid userId, Guid accountId, string symbol, long quantity, long nonce);

        /// <summary>
        /// Sells the supplied quantity of shares at the current market price.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the trading account.</param>
        /// <param name="accountId">The unique identifier of the account.</param>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="quantity">The quantity to sell.</param>
        /// <param name="nonce">The nonce value required to detect suplicate orders.</param>
        void SellSharesAtMarketPrice(Guid userId, Guid accountId, string symbol, long quantity, long nonce);
    }
}
