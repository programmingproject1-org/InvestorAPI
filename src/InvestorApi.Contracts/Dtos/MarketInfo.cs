using System;

namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Provides information about a market.
    /// </summary>
    public class MarketInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketInfo"/> class.
        /// </summary>
        /// <param name="currentTime">The current local time.</param>
        /// <param name="isOpen">A value indicating whether the market is currently open.</param>
        /// <param name="timeUntilNextOpen">The time until the market opens.</param>
        /// <param name="timeUntilNextClose">The time until the market closes.</param>
        public MarketInfo(DateTimeOffset currentTime, bool isOpen, TimeSpan timeUntilNextOpen, TimeSpan timeUntilNextClose)
        {
            CurrentTime = currentTime;
            IsOpen = IsOpen;
            TimeUntilNextOpen = timeUntilNextOpen;
            TimeUntilNextClose = timeUntilNextClose;
        }

        /// <summary>
        /// Gets the current local time.
        /// </summary>
        public DateTimeOffset CurrentTime { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the market is currently open.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Gets the time until the market opens.
        /// </summary>
        public TimeSpan TimeUntilNextOpen { get; private set; }

        /// <summary>
        /// Gets the time until the market closes.
        /// </summary>
        public TimeSpan TimeUntilNextClose { get; private set; }
    }
}
