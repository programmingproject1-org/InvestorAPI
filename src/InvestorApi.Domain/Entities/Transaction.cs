using InvestorApi.Contracts;
using System;

namespace InvestorApi.Domain.Entities
{
    public class Transaction
    {
        private Transaction()
        {
            // Required for instantiation by repository.
        }

        private Transaction(Guid id, Guid accountId, DateTime timestampUtc, string description, decimal amount, decimal balance)
        {
            Id = id;
            AccountId = accountId;
            TimestampUtc = timestampUtc;
            Description = description;
            Amount = amount;
            Balance = balance;
        }

        public Guid Id { get; private set; }

        public Guid AccountId { get; private set; }

        public DateTime TimestampUtc { get; private set; }

        public string Description { get; private set; }

        public decimal Amount { get; private set; }

        public decimal Balance { get; private set; }

        public static Transaction Create(Account account, string description, decimal amount, decimal balance)
        {
            return new Transaction(Guid.NewGuid(), account.Id, DateTime.UtcNow, description, amount, balance);
        }

        internal TransactionInfo ToTransactionInfo()
        {
            return new TransactionInfo(TimestampUtc, Description, Amount, Balance);
        }
    }
}
