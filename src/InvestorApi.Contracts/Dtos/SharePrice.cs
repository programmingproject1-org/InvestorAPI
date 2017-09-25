using System;

namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// S historical share price.
    /// </summary>
    public class SharePrice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharePrice"/> class.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="open">The opening price.</param>
        /// <param name="high">The highest price in the period.</param>
        /// <param name="low">The lowest price in the period.</param>
        /// <param name="close">The closing price.</param>
        public SharePrice(DateTime timestamp, decimal open, decimal high, decimal low, decimal close)
        {
            Timestamp = timestamp;
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Gets the opening price.
        /// </summary>
        public decimal Open { get; private set; }

        /// <summary>
        /// Gets the highest price in the period.
        /// </summary>
        public decimal High { get; private set; }

        /// <summary>
        /// Gets the lowest price in the period.
        /// </summary>
        /// <value>
        /// The low.
        /// </value>
        public decimal Low { get; private set; }

        /// <summary>
        /// Gets the closing price.
        /// </summary>
        public decimal Close { get; private set; }
    }
}
