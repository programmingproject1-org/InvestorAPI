using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Contracts.Settings;
using InvestorApi.Domain.Exceptions;
using InvestorApi.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.Entities
{
    /// <summary>
    /// Encapsulates the domain logic for a trading account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="Account"/> class from being created.
        /// </summary>
        /// <remarks>
        /// This default constructor must not be removed!
        /// It is required by the repository to instantiate new instances of the class.
        /// </remarks>
        private Account()
        {
            Positions = new List<Position>();
            Transactions = new List<Transaction>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the account.</param>
        /// <param name="userId">The unique identifier of the user who owns the account.</param>
        /// <param name="name">The name of the account.</param>
        private Account(Guid id, Guid userId, string name)
            : this()
        {
            Id = id;
            UserId = userId;
            Name = name;
            Balance = 0;
            LastNonce = 0;
        }

        /// <summary>
        /// Gets the unique identifier of the account.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the user who owns the account.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the name of the account.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the current cash balance.
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Gets the last used nonce.
        /// </summary>
        public long LastNonce { get; private set; }

        /// <summary>
        /// Gets the account positions.
        /// </summary>
        public ICollection<Position> Positions { get; private set; }

        /// <summary>
        /// Gets the account transactions.
        /// </summary>
        public ICollection<Transaction> Transactions { get; private set; }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the account.</param>
        /// <param name="name">The name of the account.</param>
        /// <param name="initialBalance">The initial account balance.</param>
        /// <returns>The newly created account.</returns>
        public static Account CreateNew(Guid userId, string name, decimal initialBalance)
        {
            Validate.NotEmpty(userId, nameof(userId));
            Validate.NotNullOrWhitespace(name, nameof(name));

            Account account = new Account(Guid.NewGuid(), userId, name);
            account.Reset(initialBalance, "Account opened");
            return account;
        }

        /// <summary>
        /// Resets the account to its initial balance.
        /// </summary>
        /// <param name="initialBalance">The initial account balance.</param>
        /// <param name="transactionName">The name of the initial account balance transaction.</param>
        public void Reset(decimal initialBalance, string transactionName)
        {
            Validate.NotNullOrWhitespace(transactionName, nameof(transactionName));

            LastNonce = 0;

            Positions.Clear();

            Transactions.Clear();
            Transactions.Add(Transaction.Create(this, TransactionType.Transfer, transactionName, initialBalance, initialBalance));

            Balance = initialBalance;
        }

        /// <summary>
        /// Buys the supplied quantity of shares at the current market price.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="quantity">The quantity to buy.</param>
        /// <param name="price">The purchase price per share.</param>
        /// <param name="commissions">The commissions to apply.</param>
        /// <param name="nonce">The nonce value required to detect duplicate orders.</param>
        public void BuyShares(string symbol, long quantity, decimal price, Commissions commissions, long nonce)
        {
            Validate.NotNullOrWhitespace(symbol, nameof(symbol));
            Validate.Minimum(quantity, 1, nameof(quantity));
            Validate.Minimum(price, 0.001m, nameof(price));
            Validate.NotNull(commissions, nameof(commissions));

            // Check the nonce value. This will though an exception if invalid.
            CheckNonce(nonce);

            // Calculate all the individual amounts.
            decimal amount = quantity * price;
            decimal fixedCommissionAmount = GetCommission(commissions.Fixed, amount);
            decimal percentageCommission = GetCommission(commissions.Percentage, amount);
            decimal percentageCommissionAmount = amount * percentageCommission / 100;
            decimal totalFees = percentageCommissionAmount + fixedCommissionAmount;
            decimal totalAmount = amount + totalFees;

            if (totalAmount > Balance)
            {
                throw new InvalidTradeException($"The total amount of the transaction exceeds the current account balance by ${(totalAmount - Balance):N2}.");
            }

            // Find an existing position to update. If none exist, create a new one.
            Position position = Positions.FirstOrDefault(p => p.Symbol == symbol);
            if (position != null)
            {
                position.Buy(quantity, price, totalFees);
            }
            else
            {
                position = Position.Create(this, symbol, quantity, price, totalFees);
                Positions.Add(position);
            }

            // Create the transaction records and update the balances.
            Balance = Balance - amount;
            Transactions.Add(Transaction.Create(this, TransactionType.Buy, $"Purchased {quantity} shares of {symbol} for ${price:N3} each", -amount, Balance));

            Balance = Balance - percentageCommissionAmount;
            Transactions.Add(Transaction.Create(this, TransactionType.Commission, $"Commission {percentageCommission:N2}%", -percentageCommissionAmount, Balance));

            Balance = Balance - fixedCommissionAmount;
            Transactions.Add(Transaction.Create(this, TransactionType.Commission, $"Commission", -fixedCommissionAmount, Balance));
        }

        /// <summary>
        /// Sells the supplied quantity of shares at the current market price.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="quantity">The quantity to sell.</param>
        /// <param name="price">The sell price per share.</param>
        /// <param name="commissions">The commissions to apply.</param>
        /// <param name="nonce">The nonce value required to detect duplicate orders.</param>
        public void SellShares(string symbol, long quantity, decimal price, Commissions commissions, long nonce)
        {
            Validate.NotNullOrWhitespace(symbol, nameof(symbol));
            Validate.Minimum(quantity, 1, nameof(quantity));
            Validate.Minimum(price, 0.001m, nameof(price));
            Validate.NotNull(commissions, nameof(commissions));

            // Check the nonce value. This will though an exception if invalid.
            CheckNonce(nonce);

            // Calculate all the individual amounts.
            decimal amount = quantity * price;
            decimal fixedCommissionAmount = GetCommission(commissions.Fixed, amount);
            decimal percentageCommission = GetCommission(commissions.Percentage, amount);
            decimal percentageCommissionAmount = amount * percentageCommission / 100;
            decimal totalFees = percentageCommissionAmount + fixedCommissionAmount;
            decimal totalAmount = totalFees - amount;

            if (totalAmount > Balance)
            {
                throw new InvalidTradeException($"The total amount of the transction exceeds the current account balance by ${(totalAmount - Balance):N2}.");
            }

            // Update the position.
            Position position = Positions.FirstOrDefault(p => p.Symbol == symbol);
            if (position == null || position.Quantity < quantity)
            {
                throw new InvalidTradeException($"You cannot sell {quantity} shares of {symbol} because the current position is only {position?.Quantity ?? 0}.");
            }

            position.Sell(quantity);

            if (position.Quantity == 0)
            {
                Positions.Remove(position);
            }

            // Create the transaction records and update the balances.
            Balance = Balance + amount;
            Transactions.Add(Transaction.Create(this, TransactionType.Sell, $"Sold {quantity} shares of {symbol} for ${price:N3} each", amount, Balance));

            Balance = Balance - percentageCommissionAmount;
            Transactions.Add(Transaction.Create(this, TransactionType.Commission, $"Commission {percentageCommission:N2}%", -percentageCommissionAmount, Balance));

            Balance = Balance - fixedCommissionAmount;
            Transactions.Add(Transaction.Create(this, TransactionType.Commission, $"Commission", -fixedCommissionAmount, Balance));
        }

        /// <summary>
        /// Exports the state of the entity to a new instance of <see cref="AccountInfo"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="AccountInfo"/> with the current state of the entity.</returns>
        internal AccountInfo ToAccountInfo()
        {
            return new AccountInfo(Id, Name, Balance);
        }

        private void CheckNonce(long nonce)
        {
            if (nonce <= LastNonce)
            {
                throw new InvalidTradeException("The nonce value is invalid. It might indicate a duplicate order.");
            }

            LastNonce = nonce;
        }

        private decimal GetCommission(IEnumerable<CommissionRange> commissionRanges, decimal amount)
        {
            // Find the range into which the amount falls.
            var range = commissionRanges.FirstOrDefault(c => c.Min <= amount && c.Max >= amount);
            if (range == null)
            {
                var max = commissionRanges.Max(c => c.Max);
                if (amount > max)
                {
                    throw new InvalidTradeException($"The amount exceeds the permitted maximum of ${max:N2}.");
                }

                throw new InvalidTradeException($"No commission range found for amount ${amount:N2}.");
            }

            return range.Value;
        }
    }
}
