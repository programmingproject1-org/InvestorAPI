using InvestorApi.Contracts.Settings;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace InvestorApi.UnitTests.Entities
{
    public class AccountTests
    {
        private static readonly Commissions Commissions = new Commissions
        {
            Fixed = new List<CommissionRange>
            {
                new CommissionRange { Min = 0, Max = 10000, Value = 50 }
            },
            Percentage = new List<CommissionRange>
            {
                new CommissionRange { Min = 0, Max = 10000, Value = 2 }
            }
        };

        [Fact]
        public void Create_Success()
        {
            Guid userId = Guid.NewGuid();
            decimal amount = 5000;

            Account account = Account.CreateNew(userId, "Test Account", amount);

            Assert.Equal(userId, account.UserId);
            Assert.Equal(amount, account.Balance);
            Assert.Equal(1, account.Transactions.Count);
            Assert.Equal(amount, account.Transactions.First().Amount);
            Assert.Equal(amount, account.Transactions.First().Balance);
        }

        [Fact]
        public void Reset_Success()
        {
            decimal amount = 5000;

            Account account = Account.CreateNew(Guid.NewGuid(), "Test Account", amount * 2);
            account.Reset(amount);

            Assert.Equal(amount, account.Balance);
            Assert.Equal(1, account.Transactions.Count);
            Assert.Equal(amount, account.Transactions.First().Amount);
            Assert.Equal(amount, account.Transactions.First().Balance);
        }

        [Fact]
        public void BuyShares_Success()
        {
            Account account = Account.CreateNew(Guid.NewGuid(), "Test Account", 10000);
            account.BuyShares("AAA", 100, 50, Commissions, 1);

            Assert.Equal(4850, account.Balance);
            Assert.Equal(4, account.Transactions.Count);
            Assert.Equal(1, account.Positions.Count);
        }

        [Fact]
        public void BuyShares_InvalidNonce()
        {
            Account account = Account.CreateNew(Guid.NewGuid(), "Test Account", 10000);

            Assert.Throws<InvalidTradeException>(() => account.BuyShares("AAA", 100, 50, Commissions, 0));
        }

        [Fact]
        public void BuyShares_BalanceToLow()
        {
            Account account = Account.CreateNew(Guid.NewGuid(), "Test Account", 5000);

            Assert.Throws<InvalidTradeException>(() => account.BuyShares("AAA", 100, 50, Commissions, 0));
        }

        [Fact]
        public void BuyShares_NoCommissionRange()
        {
            Account account = Account.CreateNew(Guid.NewGuid(), "Test Account", 10000);

            Assert.Throws<InvalidTradeException>(() => account.BuyShares("AAA", 1000, 50, Commissions, 0));
        }

        [Fact]
        public void SellShares_Success()
        {
            Account account = Account.CreateNew(Guid.NewGuid(), "Test Account", 10000);
            account.BuyShares("AAA", 100, 50, Commissions, 1);
            account.SellShares("AAA", 100, 50, Commissions, 2);

            Assert.Equal(9700, account.Balance);
            Assert.Equal(7, account.Transactions.Count);
            Assert.Equal(0, account.Positions.Count);
        }

        [Fact]
        public void SellShares_InvalidNonce()
        {
            Account account = Account.CreateNew(Guid.NewGuid(), "Test Account", 10000);
            account.BuyShares("AAA", 100, 50, Commissions, 1);

            Assert.Throws<InvalidTradeException>(() => account.SellShares("AAA", 100, 50, Commissions, 1));
        }

        [Fact]
        public void SellShares_QuantityToHigh()
        {
            Account account = Account.CreateNew(Guid.NewGuid(), "Test Account", 10000);
            account.BuyShares("AAA", 100, 50, Commissions, 1);

            Assert.Throws<InvalidTradeException>(() => account.SellShares("AAA", 110, 50, Commissions, 2));
        }
    }
}
