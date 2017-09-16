using System;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// Contains summary information about a trading account transaction.
    /// </summary>
    public class TransactionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionInfo" /> class.
        /// </summary>
        /// <param name="timestampUtc">The date and time when the transaction occurred in UTC.</param>
        /// <param name="type">The type of the transaction.</param>
        /// <param name="description">The description.</param>
        /// <param name="amount">The transaction amount.</param>
        /// <param name="balance">The account balance after the transaction.</param>
        public TransactionInfo(DateTime timestampUtc, TransactionType type, string description, decimal amount, decimal balance)
        {
            TimestampUtc = timestampUtc;
            Type = type;
            Description = description;
            Amount = amount;
            Balance = balance;
        }

        /// <summary>
        /// Gets the date and time when the transaction occurred in UTC.
        /// </summary>
        public DateTime TimestampUtc { get; private set; }

        /// <summary>
        /// Gets the type of the transaction.
        /// </summary>
        public TransactionType Type { get; private set; }

        /// <summary>
        /// Gets the description.
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
    }
}
