using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Exceptions;
using InvestorApi.Domain.Repositories;
using System;

namespace InvestorApi.Domain.Services
{
    internal class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public AccountDetails GetAccountDetails(Guid userId, Guid accountId)
        {
            throw new NotImplementedException();
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
            Account account = Account.CreateNew(userId, name ?? "Default Account", 1000000);
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
            Account account = GetAccount(userId, accountId);
            account.Reset(1000000);
            _accountRepository.Save(account);
        }

        public void BuySharesAtMarketPrice(Guid userId, Guid accountId, string symbol, int quantity)
        {
            Account account = GetAccount(userId, accountId);
            account.BuyShares(symbol, quantity, 10m);
            _accountRepository.Save(account);
        }

        public void SellSharesAtMarketPrice(Guid userId, Guid accountId, string symbol, int quantity)
        {
            Account account = GetAccount(userId, accountId);
            account.SellShares(symbol, quantity, 10m);
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
