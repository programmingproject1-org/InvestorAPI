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
        /// <param name="volume">The traded volume.</param>
        public SharePrice(DateTimeOffset timestamp, decimal? open, decimal? high, decimal? low, decimal? close, long? volume)
        {
            Timestamp = timestamp;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        public DateTimeOffset Timestamp { get; private set; }

        /// <summary>
        /// Gets the opening price.
        /// </summary>
        public decimal? Open { get; private set; }

        /// <summary>
        /// Gets the highest price in the period.
        /// </summary>
        public decimal? High { get; private set; }

        /// <summary>
        /// Gets the lowest price in the period.
        /// </summary>
        public decimal? Low { get; private set; }

        /// <summary>
        /// Gets the closing price.
        /// </summary>
        public decimal? Close { get; private set; }

        /// <summary>
        /// Gets the traded volume.
        /// </summary>
        public long? Volume { get; private set; }
    }
}
