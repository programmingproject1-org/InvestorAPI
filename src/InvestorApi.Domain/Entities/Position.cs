using InvestorApi.Domain.Utilities;
using System;

namespace InvestorApi.Domain.Entities
{
    /// <summary>
    /// Encapsulates the domain logic for a trading account position.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="Position"/> class from being created.
        /// </summary>
        /// <remarks>
        /// This default constructor must not be removed!
        /// It is required by the repository to instantiate new instances of the class.
        /// </remarks>
        private Position()
        {
            // Required for instantiation by repository.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the position.</param>
        /// <param name="accountId">The unique identifier of the account to which the position belongs.</param>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="quantity">The position quantity.</param>
        /// <param name="averagePrice">The average price paid per share owned.</param>
        private Position(Guid id, Guid accountId, string symbol, long quantity, decimal averagePrice)
        {
            Id = id;
            AccountId = accountId;
            Symbol = symbol;
            Quantity = quantity;
            AveragePrice = averagePrice;
        }

        /// <summary>
        /// Gets the unique identifier of the position.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the account to which the position belongs.
        /// </summary>
        public Guid AccountId { get; private set; }

        /// <summary>
        /// Gets the share symbol.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// Gets the position quantity.
        /// </summary>
        public long Quantity { get; private set; }

        /// <summary>
        /// Gets the average price paid per share.
        /// </summary>
        public decimal AveragePrice { get; private set; }

        /// <summary>
        /// Creates a new position for a boght share.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="quantity">The position quantity.</param>
        /// <param name="price">The price paid per share.</param>
        /// <param name="brokerageFees">The brokerage fees paid.</param>
        /// <returns>The newly created position.</returns>
        public static Position Create(Account account, string symbol, long quantity, decimal price, decimal brokerageFees)
        {
            Validate.NotNull(account, nameof(account));
            Validate.NotNullOrWhitespace(symbol, nameof(symbol));
            Validate.Minimum(quantity, 1, nameof(quantity));
            Validate.Minimum(price, 0.001m, nameof(price));
            Validate.Minimum(brokerageFees, 0m, nameof(brokerageFees));

            decimal averagePrice = ((quantity * price) + brokerageFees) / quantity;
            return new Position(Guid.NewGuid(), account.Id, symbol, quantity, averagePrice);
        }

        /// <summary>
        /// Adds additional quantities of a share to the position.
        /// </summary>
        /// <param name="additionalQuantity">The additional quantity.</param>
        /// <param name="price">The price paid per share.</param>
        /// <param name="brokerageFees">The brokerage fees paid.</param>
        public void Buy(long additionalQuantity, decimal price, decimal brokerageFees)
        {
            Validate.Minimum(additionalQuantity, 1, nameof(additionalQuantity));
            Validate.Minimum(price, 0.001m, nameof(price));
            Validate.Minimum(brokerageFees, 0m, nameof(brokerageFees));

            // Calculate the new average price.
            AveragePrice = ((Quantity * AveragePrice) + (additionalQuantity * price) + brokerageFees) / (Quantity + additionalQuantity);
            Quantity += additionalQuantity;
        }

        /// <summary>
        /// Sells the specified quantity of shares.
        /// </summary>
        /// <param name="quantity">The quantity sold.</param>
        public void Sell(long quantity)
        {
            Validate.Minimum(quantity, 1, nameof(quantity));

            Quantity -= quantity;
        }
    }
}
