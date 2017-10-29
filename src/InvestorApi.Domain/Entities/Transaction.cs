using InvestorApi.Contracts;
using InvestorApi.Domain.Utilities;
using System;

namespace InvestorApi.Domain.Entities
{
    /// <summary>
    /// Encapsulates the domain logic for a transaction.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="Transaction"/> class from being created.
        /// </summary>
        /// <remarks>
        /// This default constructor must not be removed!
        /// It is required by the repository to instantiate new instances of the class.
        /// </remarks>
        private Transaction()
        {
            // Required for instantiation by repository.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the transaction.</param>
        /// <param name="accountId">The unique identifier of the account on which the transaction occurred.</param>
        /// <param name="timestampUtc">The date and time in UTC when the transaction occurred.</param>
        /// <param name="type">The type of transaction.</param>
        /// <param name="description">The description of the transaction.</param>
        /// <param name="amount">The transaction amount.</param>
        /// <param name="balance">The account balance after the transaction.</param>
        private Transaction(Guid id, Guid accountId, DateTime timestampUtc, TransactionType type, string description, decimal amount, decimal balance)
        {
            Id = id;
            AccountId = accountId;
            TimestampUtc = timestampUtc;
            Type = type;
            Description = description;
            Amount = amount;
            Balance = balance;
        }

        /// <summary>
        /// Gets the unique identifier of the transaction.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the account on which the transaction occurred.
        /// </summary>
        public Guid AccountId { get; private set; }

        /// <summary>
        /// Gets the date and time in UTC when the transaction occurred.
        /// </summary>
        public DateTime TimestampUtc { get; private set; }

        /// <summary>
        /// Gets the type of transaction.
        /// </summary>
        public TransactionType Type { get; private set; }

        /// <summary>
        /// Gets the description of the transaction.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the transaction amount.
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Gets the account balance after the transaction.
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="account">The account of the transaction.</param>
        /// <param name="type">The type of transaction.</param>
        /// <param name="description">The description of the transaction.</param>
        /// <param name="amount">The transaction amount.</param>
        /// <param name="balance">The balance after the transaction.</param>
        /// <returns>The newly created transaction.</returns>
        public static Transaction Create(Account account, TransactionType type, string description, decimal amount, decimal balance)
        {
            Validate.NotNull(account, nameof(account));
            Validate.NotNullOrWhitespace(description, nameof(description));

            return new Transaction(Guid.NewGuid(), account.Id, DateTime.UtcNow, type, description, amount, balance);
        }

        /// <summary>
        /// Exports the state of the entity to a new instance of <see cref="TransactionInfo"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="TransactionInfo"/> with the current state of the entity.</returns>
        internal TransactionInfo ToTransactionInfo()
        {
            return new TransactionInfo(TimestampUtc, Type, Description, Amount, Balance);
        }
    }
}
