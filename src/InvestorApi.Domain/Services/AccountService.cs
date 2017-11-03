using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Contracts.Settings;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Exceptions;
using InvestorApi.Domain.Repositories;
using InvestorApi.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.Services
{
    /// <summary>
    /// A domain service to manage trading accounts.
    /// </summary>
    internal class AccountService : IAccountService
    {
        private readonly ISettingService _settingService;
        private readonly IAccountRepository _accountRepository;
        private readonly IShareQuoteProvider _shareQuoteProvider;
        private readonly IShareInfoProvider _shareInfoProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="settingService">The setting service.</param>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="shareQuoteProvider">The share quote provider.</param>
        /// <param name="shareInfoProvider">The share info provider.</param>
        public AccountService(
            ISettingService settingService,
            IAccountRepository accountRepository,
            IShareQuoteProvider shareQuoteProvider,
            IShareInfoProvider shareInfoProvider)
        {
            _settingService = settingService;
            _accountRepository = accountRepository;
            _shareQuoteProvider = shareQuoteProvider;
            _shareInfoProvider = shareInfoProvider;
        }

        /// <summary>
        /// Gets detailed information about a specific trading accounts.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve the trading account for.</param>
        /// <param name="accountId">The unique identifier of the account to return.</param>
        /// <returns>The trading account details.</returns>
        public AccountDetails GetAccountDetails(Guid userId, Guid accountId)
        {
            Validate.NotEmpty(userId, nameof(userId));
            Validate.NotEmpty(accountId, nameof(accountId));

            Account account = GetAccount(userId, accountId);
            return GetAccountDetails(account);
        }

        /// <summary>
        /// Gets detailed information about a specific trading accounts.
        /// </summary>
        /// <param name="account">The trading account to return the details for.</param>
        /// <returns>The trading account details.</returns>
        public AccountDetails GetAccountDetails(Account account)
        {
            Validate.NotNull(account, nameof(account));

            IReadOnlyDictionary<string, ShareInfo> shareDetails = _shareInfoProvider
                .GetShareInfo(account.Positions.Select(position => position.Symbol));

            IReadOnlyDictionary<string, Quote> quotes = _shareQuoteProvider
                .GetQuotes(account.Positions.Select(position => position.Symbol));

            List<PositionInfo> positions = account.Positions
                .Where(p => shareDetails.ContainsKey(p.Symbol) && quotes.ContainsKey(p.Symbol))
                .Select(p => new
                {
                    P = p,
                    D = shareDetails[p.Symbol],
                    Q = quotes[p.Symbol],
                })
                .Select(i => new PositionInfo(i.D.Symbol, i.D.Name, i.P.Quantity, i.P.AveragePrice, i.Q.Last, i.Q.Change, i.Q.ChangePercent))
                .ToList();

            return new AccountDetails(account.Id, account.Name, account.Balance, positions);
        }

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
        public ListResult<TransactionInfo> ListTransactions(Guid userId, Guid accountId, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            Validate.NotEmpty(userId, nameof(userId));
            Validate.NotEmpty(accountId, nameof(accountId));
            Validate.Range(pageNumber, 1, 1000, nameof(pageNumber));
            Validate.Range(pageSize, 1, 100, nameof(pageSize));

            // Verify that account exists and belongs to the user.
            GetAccount(userId, accountId);

            var result = _accountRepository.ListTransactions(accountId, startDate, endDate, pageNumber, pageSize);
            return result.Convert(transaction => transaction.ToTransactionInfo());
        }

        /// <summary>
        /// Opens the new trading account.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the trading account.</param>
        /// <param name="name">The account name.</param>
        /// <returns>The identifier of the newly created account.</returns>
        public Guid CreateAccount(Guid userId, string name)
        {
            Validate.NotEmpty(userId, nameof(userId));
            Validate.NotNullOrWhitespace(name, nameof(name));

            DefaultAccountSettings settings = _settingService.GetDefaultAccountSettings();

            Account account = Account.CreateNew(userId, name, settings.InitialBalance);
            _accountRepository.Save(account);
            return account.Id;
        }

        /// <summary>
        /// Delete an existing trading account.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the trading account.</param>
        /// <param name="accountId">The unique identifier of the account to delete.</param>
        public void DeleteAccount(Guid userId, Guid accountId)
        {
            Validate.NotEmpty(userId, nameof(userId));
            Validate.NotEmpty(accountId, nameof(accountId));

            Account account = GetAccount(userId, accountId);
            _accountRepository.Delete(account);
        }

        /// <summary>
        /// Resets a trading account to its starting state.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the trading account.</param>
        /// <param name="accountId">The unique identifier of the account to reset.</param>
        public void ResetAccount(Guid userId, Guid accountId)
        {
            Validate.NotEmpty(userId, nameof(userId));
            Validate.NotEmpty(accountId, nameof(accountId));

            DefaultAccountSettings settings = _settingService.GetDefaultAccountSettings();

            Account account = GetAccount(userId, accountId);
            account.Reset(settings.InitialBalance, "Account reset");
            _accountRepository.Reset(account);
        }

        /// <summary>
        /// Buys the supplied quantity of shares at the current market price.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the trading account.</param>
        /// <param name="accountId">The unique identifier of the account.</param>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="quantity">The quantity to buy.</param>
        /// <param name="nonce">The nonce value required to detect duplicate orders.</param>
        public void BuySharesAtMarketPrice(Guid userId, Guid accountId, string symbol, long quantity, long nonce)
        {
            Validate.NotEmpty(userId, nameof(userId));
            Validate.NotEmpty(accountId, nameof(accountId));
            Validate.NotNullOrWhitespace(symbol, nameof(symbol));

            Quote quote = _shareQuoteProvider.GetQuote(symbol);
            if (quote == null)
            {
                throw new EntityNotFoundException($"Share with symbol '{symbol}' not found.");
            }

            Commissions commissions = _settingService.GetBuyCommissions();

            Account account = GetAccount(userId, accountId);
            account.BuyShares(symbol, quantity, quote.Ask, commissions, nonce);
            _accountRepository.Save(account);
        }

        /// <summary>
        /// Sells the supplied quantity of shares at the current market price.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the trading account.</param>
        /// <param name="accountId">The unique identifier of the account.</param>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="quantity">The quantity to sell.</param>
        /// <param name="nonce">The nonce value required to detect duplicate orders.</param>
        public void SellSharesAtMarketPrice(Guid userId, Guid accountId, string symbol, long quantity, long nonce)
        {
            Validate.NotEmpty(userId, nameof(userId));
            Validate.NotEmpty(accountId, nameof(accountId));
            Validate.NotNullOrWhitespace(symbol, nameof(symbol));

            Quote quote = _shareQuoteProvider.GetQuote(symbol);
            if (quote == null)
            {
                throw new EntityNotFoundException($"Share with symbol '{symbol}' not found.");
            }

            Commissions commissions = _settingService.GetSellCommissions();

            Account account = GetAccount(userId, accountId);
            account.SellShares(symbol, quantity, quote.Bid, commissions, nonce);
            _accountRepository.Save(account);
        }

        private Account GetAccount(Guid userId, Guid accountId)
        {
            Account account = _accountRepository.GetById(accountId);

            if (account == null || account.UserId != userId)
            {
                throw new EntityNotFoundException(nameof(Account), accountId);
            }

            return account;
        }
    }
}
