using InvestorApi.Domain.Entities;
using System;

namespace InvestorApi.Domain.Repositories
{
    public interface IWatchlistRepository
    {
        /// <summary>
        /// Gets a watchlist by its unique identifier.
        /// </summary>
        /// <param name="watchlistId">The watchlist identifier.</param>
        /// <returns>The matching watchlist.</returns>
        Watchlist GetById(Guid watchlistId);

        /// <summary>
        /// Saves the specified watchlist.
        /// </summary>
        /// <param name="watchlist">The watchlist to save.</param>
        void Save(Watchlist watchlist);

        /// <summary>
        /// Deletes the specified watchlist.
        /// </summary>
        /// <param name="watchlist">The watchlist to delete.</param>
        void Delete(Watchlist watchlist);
    }
}
