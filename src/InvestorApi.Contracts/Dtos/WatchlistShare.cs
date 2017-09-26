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
        /// <param name="change">The day's change.</param>
        /// <param name="changePercent">The day's change in percent.</param>
        public WatchlistShare(string symbol, string name, decimal lastPrice, decimal change, decimal changePercent)
        {
            Symbol = symbol;
            Name = name;
            LastPrice = lastPrice;
            Change = change;
            ChangePercent = changePercent;
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

        /// <summary>
        /// Gets the day's change.
        /// </summary>
        public decimal Change { get; private set; }

        /// <summary>
        /// Gets the day's change in percent.
        /// </summary>
        public decimal ChangePercent { get; private set; }
    }
}
