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
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public AccountDetails GetAccountDetails(Guid userId, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Guid CreateAccount(Guid userId, string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteAccount(Guid userId, Guid accountId)
        {
            Account account = GetAccount(userId, accountId);
            _accountRepository.Delete(account);
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
