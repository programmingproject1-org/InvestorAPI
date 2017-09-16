using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Contracts.Settings;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Exceptions;
using InvestorApi.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.Services
{
    internal class AccountService : IAccountService
    {
        private readonly ISettingService _settingService;
        private readonly IAccountRepository _accountRepository;
        private readonly IShareQuoteProvider _shareQuoteProvider;
        private readonly IShareDetailsProvider _shareDetailsProvider;

        public AccountService(
            ISettingService settingService,
            IAccountRepository accountRepository,
            IShareQuoteProvider shareQuoteProvider,
            IShareDetailsProvider shareDetailsProvider)
        {
            _settingService = settingService;
            _accountRepository = accountRepository;
            _shareQuoteProvider = shareQuoteProvider;
            _shareDetailsProvider = shareDetailsProvider;
        }

        public AccountDetails GetAccountDetails(Guid userId, Guid accountId)
        {
            Account account = GetAccount(userId, accountId);

            IReadOnlyDictionary<string, ShareDetails> shareDetails = _shareDetailsProvider
                .GetShareDetails(account.Positions.Select(position => position.Symbol));

            IReadOnlyDictionary<string, Quote> quotes = _shareQuoteProvider
                .GetQuotes(account.Positions.Select(position => position.Symbol));

            List<PositionInfo> positions = account.Positions
                .Select(p => new
                {
                    Position = p,
                    Detail = shareDetails[p.Symbol],
                    Quote = quotes[p.Symbol],
                })
                .Select(i => new PositionInfo(i.Detail.Symbol, i.Detail.Name, i.Position.Quantity, i.Position.AveragePrice, i.Quote.LastPrice))
                .ToList();

            return new AccountDetails(account.Id, account.Name, account.Balance, positions);
        }

        public ListResult<TransactionInfo> ListTransactions(Guid userId, Guid accountId, int pageNumber, int pageSize)
        {
            // Verify that account exists and belongs to the user.
            GetAccount(userId, accountId);

            var result = _accountRepository.ListTransactions(accountId, pageNumber, pageSize);
            return result.Convert(transaction => transaction.ToTransactionInfo());
        }

        public Guid CreateAccount(Guid userId, string name)
        {
            DefaultAccountSettings settings = _settingService.GetDefaultAccountSettings();

            Account account = Account.CreateNew(userId, name ?? settings.Name, settings.InitialBalance);
            _accountRepository.Save(account);
            return account.Id;
        }

        public void DeleteAccount(Guid userId, Guid accountId)
        {
            Account account = GetAccount(userId, accountId);
            _accountRepository.Delete(account);
        }

        public void ResetAccount(Guid userId, Guid accountId)
        {
            DefaultAccountSettings settings = _settingService.GetDefaultAccountSettings();

            Account account = GetAccount(userId, accountId);
            account.Reset(settings.InitialBalance);
            _accountRepository.Save(account);
        }

        public void BuySharesAtMarketPrice(Guid userId, Guid accountId, string symbol, int quantity)
        {
            Quote quote = _shareQuoteProvider.GetQuote(symbol);
            if (quote == null)
            {
                throw new EntityNotFoundException($"Share with symbol '{symbol}' not found.");
            }

            Commissions commissions = _settingService.GetBuyCommissions();

            Account account = GetAccount(userId, accountId);
            account.BuyShares(symbol, quantity, quote.Ask, commissions);
            _accountRepository.Save(account);
        }

        public void SellSharesAtMarketPrice(Guid userId, Guid accountId, string symbol, int quantity)
        {
            Quote quote = _shareQuoteProvider.GetQuote(symbol);
            if (quote == null)
            {
                throw new EntityNotFoundException($"Share with symbol '{symbol}' not found.");
            }

            Commissions commissions = _settingService.GetSellCommissions();

            Account account = GetAccount(userId, accountId);
            account.SellShares(symbol, quantity, quote.Bid, commissions);
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
