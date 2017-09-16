namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains the current quote for a share.
    /// </summary>
    public class Quote
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Quote"/> class.
        /// </summary>
        /// <param name="ask">The current ask price.</param>
        /// <param name="bid">The current bid price.</param>
        /// <param name="lastPrice">The last paid market price.</param>
        public Quote(decimal ask, decimal bid, decimal lastPrice)
        {
            Ask = ask;
            Bid = bid;
            LastPrice = lastPrice;
        }

        /// <summary>
        /// Gets the current ask price.
        /// </summary>
        public decimal Ask { get; private set; }

        /// <summary>
        /// Gets the current bid price.
        /// </summary>
        public decimal Bid { get; private set; }

        /// <summary>
        /// Gets the last paid market price.
        /// </summary>
        public decimal LastPrice { get; private set; }
    }
}
