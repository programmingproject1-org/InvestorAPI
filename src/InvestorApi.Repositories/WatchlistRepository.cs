using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InvestorApi.Repositories
{
    /// <summary>
    /// The repository to store and retrieve <see cref="Watchlist"/> entities.
    /// </summary>
    internal sealed class WatchlistRepository : IWatchlistRepository
    {
        private readonly DataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatchlistRepository"/> class.
        /// </summary>
        /// <param name="context">The data context.</param>
        public WatchlistRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a watchlist by its unique identifier.
        /// </summary>
        /// <param name="watchlistId">The watchlist identifier.</param>
        /// <returns>The matching watchlist.</returns>
        public Watchlist GetById(Guid watchlistId)
        {
            return _context.Watchlists
                .Where(watchlist => watchlist.Id == watchlistId)
                .FirstOrDefault();
        }

        /// <summary>
        /// Saves the specified watchlist.
        /// </summary>
        /// <param name="watchlist">The watchlist to save.</param>
        public void Save(Watchlist watchlist)
        {
            // Check if the item exists and then either create or update it in the database.
            var exists = _context.Watchlists.AsNoTracking().Any(x => x.Id == watchlist.Id);
            if (exists)
            {
                _context.Watchlists.Update(watchlist);
            }
            else
            {
                _context.Watchlists.Add(watchlist);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes the specified watchlist.
        /// </summary>
        /// <param name="watchlist">The watchlist to delete.</param>
        public void Delete(Watchlist watchlist)
        {
            _context.Watchlists.Remove(watchlist);
            _context.SaveChanges();
        }
    }
}
