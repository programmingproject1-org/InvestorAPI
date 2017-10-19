using System;

namespace InvestorApi.Domain.Entities
{
    public class Position
    {
        private Position()
        {
            // Required for instantiation by repository.
        }

        private Position(Guid id, Guid accountId, string symbol, long quantity, decimal averagePrice)
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

        public long Quantity { get; private set; }

        public decimal AveragePrice { get; private set; }

        public static Position Create(Account account, string symbol, long quantity, decimal price, decimal brokerageFees)
        {
            decimal averagePrice = ((quantity * price) + brokerageFees) / quantity;
            return new Position(Guid.NewGuid(), account.Id, symbol, quantity, averagePrice);
        }

        public void Buy(long additionalQuantity, decimal price, decimal brokerageFees)
        {
            // Calculate the new average price.
            AveragePrice = ((Quantity * AveragePrice) + (additionalQuantity * price) + brokerageFees) / (Quantity + additionalQuantity);
            Quantity += additionalQuantity;
        }

        public void Sell(long quantity)
        {
            Quantity -= quantity;
        }
    }
}
