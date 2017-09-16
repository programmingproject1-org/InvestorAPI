namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains summary information about a trading account position.
    /// </summary>
    public class PositionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionInfo"/> class.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="name">The name of the share.</param>
        /// <param name="quantity">The number of shares held.</param>
        /// <param name="averagePrice">The average purchase price.</param>
        /// <param name="lastPrice">The last paid market price.</param>
        public PositionInfo(string symbol, string name, int quantity, decimal averagePrice, decimal lastPrice)
        {
            Symbol = symbol;
            Name = name;
            Quantity = quantity;
            AveragePrice = averagePrice;
            LastPrice = lastPrice;
        }

        /// <summary>
        /// Gets the share symbol.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// Gets the name of the share.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the number of shares held.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Gets the average purchase price.
        /// </summary>
        public decimal AveragePrice { get; private set; }

        /// <summary>
        /// Gets the last paid market price.
        /// </summary>
        public decimal LastPrice { get; private set; }
    }
}
