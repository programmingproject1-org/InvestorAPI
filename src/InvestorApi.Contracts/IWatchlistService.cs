using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// A domain service to manage watchlists.
    /// </summary>
    public interface IWatchlistService
    {
        /// <summary>
        /// Gets detailed information about a specific watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve the watchlist for.</param>
        /// <param name="watchlistId">The unique identifier of the watchlist to retrieve.</param>
        /// <returns>The watchlist details.</returns>
        WatchlistDetails GetWatchlistDetails(Guid userId, Guid watchlistId);

        /// <summary>
        /// Opens the new watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the watchlist.</param>
        /// <param name="name">The watchlist name.</param>
        /// <param name="symbols">The initial symbols on the watchlist.</param>
        /// <returns>The identifier of the newly created watchlist.</returns>
        Guid CreateWatchlist(Guid userId, string name, IEnumerable<string> symbols);

        /// <summary>
        /// Rename an existing watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the watchlist.</param>
        /// <param name="watchlistId">The unique identifier of the watchlist to rename.</param>
        /// <param name="name">The watchlist name.</param>
        void RenameWatchlist(Guid userId, Guid watchlistId, string name);

        /// <summary>
        /// Delete an existing watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the watchlist.</param>
        /// <param name="watchlistId">The unique identifier of the watchlist to delete.</param>
        void DeleteWatchlist(Guid userId, Guid watchlistId);

        /// <summary>
        /// Adds the supplied share to the watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the watchlist.</param>
        /// <param name="watchlistId">The unique identifier of the watchlist.</param>
        /// <param name="symbol">The share symbol.</param>
        void AddShare(Guid userId, Guid watchlistId, string symbol);

        /// <summary>
        /// Removes the supplied share from the watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the watchlist.</param>
        /// <param name="watchlistId">The unique identifier of the watchlist.</param>
        /// <param name="symbol">The share symbol.</param>
        void RemoveShare(Guid userId, Guid watchlistId, string symbol);
    }
}
