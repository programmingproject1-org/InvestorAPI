using System;

namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains summary information about a watchlist.
    /// </summary>
    public class WatchlistInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WatchlistInfo"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the watchlist.</param>
        /// <param name="name">The watchlist name.</param>
        public WatchlistInfo(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Gets the unique identifier of the watchlist.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the watchlist name.
        /// </summary>
        public string Name { get; }
    }
}
