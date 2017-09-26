namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains the current quote for a share.
    /// </summary>
    public class Quote
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Quote" /> class.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="ask">The current ask price.</param>
        /// <param name="askSize">The current ask price size.</param>
        /// <param name="bid">The current bid price.</param>
        /// <param name="bidSize">The current bid size.</param>
        /// <param name="last">The last paid market price.</param>
        /// <param name="lastSize">The last traded size.</param>
        /// <param name="change">The day's change.</param>
        /// <param name="changePercent">The day's change in percent.</param>
        /// <param name="dayLow">The day's low.</param>
        /// <param name="dayHigh">The day's high.</param>
        public Quote(string symbol, decimal ask, int askSize, decimal bid, int bidSize,
            decimal last, int lastSize, decimal change, decimal changePercent, decimal dayLow, decimal dayHigh)
        {
            Symbol = symbol;
            Ask = ask;
            AskSize = askSize;
            Bid = bid;
            BidSize = bidSize;
            Last = last;
            LastSize = lastSize;
            Change = change;
            ChangePercent = changePercent;
            DayLow = dayLow;
            DayHigh = dayHigh;
        }

        /// <summary>
        /// Gets the share symbol.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// Gets the current ask price.
        /// </summary>
        public decimal Ask { get; private set; }

        /// <summary>
        /// Gets the current ask size.
        /// </summary>
        public int AskSize { get; private set; }

        /// <summary>
        /// Gets the current bid price.
        /// </summary>
        public decimal Bid { get; private set; }

        /// <summary>
        /// Gets the current bid size.
        /// </summary>
        public int BidSize { get; private set; }

        /// <summary>
        /// Gets the last paid market price.
        /// </summary>
        public decimal Last { get; private set; }

        /// <summary>
        /// Gets the last traded size.
        /// </summary>
        public int LastSize { get; private set; }

        /// <summary>
        /// Gets the day's change.
        /// </summary>
        public decimal Change { get; private set; }

        /// <summary>
        /// Gets the day's change in percent.
        /// </summary>
        public decimal ChangePercent { get; private set; }

        /// <summary>
        /// Gets the day's low.
        /// </summary>
        public decimal DayLow { get; private set; }

        /// <summary>
        /// Gets the day;s high.
        /// </summary>
        public decimal DayHigh { get; private set; }
    }
}
