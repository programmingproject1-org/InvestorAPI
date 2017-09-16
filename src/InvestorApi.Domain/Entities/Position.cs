using InvestorApi.Contracts.Dtos;
using System;

namespace InvestorApi.Domain.Entities
{
    public class Position
    {
        private Position()
        {
            // Required for instantiation by repository.
        }

        private Position(Guid id, Guid accountId, string symbol, int quantity, decimal averagePrice)
        {
            Id = id;
            AccountId = accountId;
            Symbol = symbol;
            Quantity = quantity;
            AveragePrice = averagePrice;
        }

        public Guid Id { get; private set; }

        public Guid AccountId { get; private set; }

        public string Symbol { get; private set; }

        public int Quantity { get; private set; }

        public decimal AveragePrice { get; private set; }

        public static Position Create(Account account, string symbol, int quantity, decimal averagePrice)
        {
            return new Position(Guid.NewGuid(), account.Id, symbol, quantity, averagePrice);
        }

        public void Buy(int additionalQuantity, decimal price, decimal brokerageFees)
        {
            AveragePrice = ((Quantity * AveragePrice) + (additionalQuantity * price) + brokerageFees) / (Quantity + additionalQuantity);
            Quantity += additionalQuantity;
        }

        public void Sell(int quantity)
        {
            Quantity -= quantity;
        }
    }
}
