using System;
using System.Collections.Generic;

namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains detailed information about a watchlist.
    /// </summary>
    public class WatchlistDetails : WatchlistInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WatchlistDetails"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the watchlist.</param>
        /// <param name="name">The watchlist name.</param>
        /// <param name="shares">The shares on the watchlist.</param>
        public WatchlistDetails(Guid id, string name, IReadOnlyCollection<WatchlistShare> shares)
            :base(id, name)
        {
            Shares = shares;
        }

        /// <summary>
        /// Gets the shares on the watchlist.
        /// </summary>
        public IReadOnlyCollection<WatchlistShare> Shares { get; }
    }
}
