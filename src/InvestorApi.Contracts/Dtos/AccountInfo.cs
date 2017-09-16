using System;

namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains summary information about a trading account.
    /// </summary>
    public class AccountInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountInfo"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the account.</param>
        /// <param name="name">The account name.</param>
        /// <param name="balance">The current account balance.</param>
        public AccountInfo(Guid id, string name, decimal balance)
        {
            Id = id;
            Name = name;
            Balance = balance;
        }

        /// <summary>
        /// Gets the unique identifier of the account.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the account name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the current account balance.
        /// </summary>
        public decimal Balance { get; }
    }
}
