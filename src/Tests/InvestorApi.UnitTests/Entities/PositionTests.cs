using InvestorApi.Domain.Entities;
using System;
using Xunit;

namespace InvestorApi.UnitTests.Entities
{
    public class PositionTests
    {
        [Fact]
        public void Create()
        {
            Account account = Account.CreateNew(Guid.NewGuid(), "Test Account", 0);
            Position position = Position.Create(account, "AAA", 100, 50, 100);

            Assert.Equal(account.Id, position.AccountId);
            Assert.Equal(100, position.Quantity);
            Assert.Equal(51, position.AveragePrice);
        }

        [Fact]
        public void Buy()
        {
            Account account = Account.CreateNew(Guid.NewGuid(), "Test Account", 0);
            Position position = Position.Create(account, "AAA", 100, 50, 100);
            position.Buy(100, 70, 100);

            Assert.Equal(200, position.Quantity);
            Assert.Equal(61, position.AveragePrice);
        }

        [Fact]
        public void Sell()
        {
            Account account = Account.CreateNew(Guid.NewGuid(), "Test Account", 0);
            Position position = Position.Create(account, "AAA", 100, 50, 100);
            position.Sell(50);

            Assert.Equal(50, position.Quantity);
            Assert.Equal(51, position.AveragePrice);
        }
    }
}
