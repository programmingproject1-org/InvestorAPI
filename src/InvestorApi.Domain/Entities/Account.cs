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
            // Required for instantiation by repository.
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
        }

        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public string Name { get; private set; }

        public decimal Balance { get; private set; }

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
            Transactions.Add(Transaction.Create(this, "Account opened", initialBalance, initialBalance));

            Balance = initialBalance;
        }

        public void BuyShares(string symbol, int quantity, decimal price)
        {
            decimal amount = quantity * price;
            decimal fixedCommission = 50m;
            decimal percentageCommissionAmount = 1m;
            decimal percentageCommission = amount * percentageCommissionAmount / 100;
            decimal totalFees = percentageCommission + fixedCommission;
            decimal totalAmount = amount + totalFees;

            if (totalAmount > Balance)
            {
                throw new InvalidTradeException($"The total amount of the transction exceeds the current account balance by ${(totalAmount - Balance):N2}.");
            }

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

            Balance = Balance - amount;
            Transactions.Add(Transaction.Create(this, $"Purchased {quantity} shares of {symbol} for ${price:N2} each", -amount, Balance));

            Balance = Balance - percentageCommission;
            Transactions.Add(Transaction.Create(this, $"Brokerage Fee {percentageCommissionAmount:N2}%", -percentageCommission, Balance));

            Balance = Balance - fixedCommission;
            Transactions.Add(Transaction.Create(this, $"Brokerage Fee", -fixedCommission, Balance));
        }

        public void SellShares(string symbol, int quantity, decimal price)
        {
            decimal amount = quantity * price;
            decimal fixedCommissionAmount = 50m;
            decimal percentageCommission = 0.25m;
            decimal percentageCommissionAmount = amount * percentageCommission / 100;
            decimal totalFees = percentageCommissionAmount + fixedCommissionAmount;
            decimal totalAmount = totalFees - amount;

            if (totalAmount > Balance)
            {
                throw new InvalidTradeException($"The total amount of the transction exceeds the current account balance by ${(totalAmount - Balance):N2}.");
            }

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

            Balance = Balance + amount;
            Transactions.Add(Transaction.Create(this, $"Sold {quantity} shares of {symbol} for ${price:N2} each", amount, Balance));

            Balance = Balance - percentageCommissionAmount;
            Transactions.Add(Transaction.Create(this, $"Brokerage Fee {percentageCommission:N2}%", -percentageCommissionAmount, Balance));

            Balance = Balance - fixedCommissionAmount;
            Transactions.Add(Transaction.Create(this, $"Brokerage Fee", -fixedCommissionAmount, Balance));
        }

        internal AccountInfo ToAccountInfo()
        {
            return new AccountInfo(Id, Name, Balance);
        }
    }
}
