namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains detailed information about a share on a watchlist.
    /// </summary>
    public class WatchlistShare
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WatchlistShare"/> class.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="name">The name of the share.</param>
        /// <param name="lastPrice">The last paid market price.</param>
        public WatchlistShare(string symbol, string name, decimal lastPrice)
        {
            Symbol = symbol;
            Name = name;
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
        /// Gets the last paid market price.
        /// </summary>
        public decimal LastPrice { get; private set; }
    }
}
