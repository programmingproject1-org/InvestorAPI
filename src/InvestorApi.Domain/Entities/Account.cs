using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Contracts.Settings;
using InvestorApi.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.Entities
{
    public class Account
    {
        private Account()
        {
            Positions = new List<Position>();
            Transactions = new List<Transaction>();
        }

        private Account(Guid id, Guid userId, string name)
            : this()
        {
            Id = id;
            UserId = userId;
            Name = name;
            Balance = 0;
            LastNonce = 0;
        }

        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public string Name { get; private set; }

        public decimal Balance { get; private set; }

        public long LastNonce { get; private set; }

        public ICollection<Position> Positions { get; private set; }

        public ICollection<Transaction> Transactions { get; private set; }

        public static Account CreateNew(Guid userId, string name, decimal initialBalance)
        {
            Account account = new Account(Guid.NewGuid(), userId, name);
            account.Reset(initialBalance);
            return account;
        }

        public void Reset(decimal initialBalance)
        {
            Positions.Clear();

            Transactions.Clear();
            Transactions.Add(Transaction.Create(this, TransactionType.Transfer, "Account opened", initialBalance, initialBalance));

            Balance = initialBalance;
        }

        public void BuyShares(string symbol, int quantity, decimal price, Commissions commissions, long nonce)
        {
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
                throw new InvalidTradeException($"The total amount of the transction exceeds the current account balance by ${(totalAmount - Balance):N2}.");
            }

            // Find an existing position to update. If none exist, create a new one.
            Position position = Positions.FirstOrDefault(p => p.Symbol == symbol);
            if (position != null)
            {
                position.Buy(quantity, price, totalFees);
            }
            else
            {
                position = Position.Create(this, symbol, quantity, price);
                Positions.Add(position);
            }

            // Create the transaction records and update the balances.
            Balance = Balance - amount;
            Transactions.Add(Transaction.Create(this, TransactionType.Buy, $"Purchased {quantity} shares of {symbol} for ${price:N2} each", -amount, Balance));

            Balance = Balance - percentageCommissionAmount;
            Transactions.Add(Transaction.Create(this, TransactionType.Commission, $"Commission {percentageCommission:N2}%", -percentageCommissionAmount, Balance));

            Balance = Balance - fixedCommissionAmount;
            Transactions.Add(Transaction.Create(this, TransactionType.Commission, $"Commission", -fixedCommissionAmount, Balance));
        }

        public void SellShares(string symbol, int quantity, decimal price, Commissions commissions, long nonce)
        {
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
            Transactions.Add(Transaction.Create(this, TransactionType.Sell, $"Sold {quantity} shares of {symbol} for ${price:N2} each", amount, Balance));

            Balance = Balance - percentageCommissionAmount;
            Transactions.Add(Transaction.Create(this, TransactionType.Commission, $"Commission {percentageCommission:N2}%", -percentageCommissionAmount, Balance));

            Balance = Balance - fixedCommissionAmount;
            Transactions.Add(Transaction.Create(this, TransactionType.Commission, $"Commission", -fixedCommissionAmount, Balance));
        }

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
