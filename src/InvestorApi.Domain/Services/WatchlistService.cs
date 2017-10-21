using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Exceptions;
using InvestorApi.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.Services
{
    /// <summary>
    /// A domain service to manage watchlists.
    /// </summary>
    internal class WatchlistService : IWatchlistService
    {
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly IShareSummaryProvider _shareDetailsProvider;
        private readonly IShareQuoteProvider _shareQuoteProvider;

        public WatchlistService(
            IWatchlistRepository watchlistRepository,
            IShareSummaryProvider shareDetailsProvider,
            IShareQuoteProvider shareQuoteProvider)
        {
            _watchlistRepository = watchlistRepository;
            _shareDetailsProvider = shareDetailsProvider;
            _shareQuoteProvider = shareQuoteProvider;
        }

        /// <summary>
        /// Gets detailed information about a specific watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve the watchlist for.</param>
        /// <param name="watchlistId">The unique identifier of the watchlist to retrieve.</param>
        /// <returns>The watchlist details.</returns>
        public WatchlistDetails GetWatchlistDetails(Guid userId, Guid watchlistId)
        {
            Watchlist watchlist = GetWatchlist(userId, watchlistId);

            IReadOnlyDictionary<string, ShareSummary> shareDetails = _shareDetailsProvider
                .GetShareSummaries(watchlist.Symbols);

            IReadOnlyDictionary<string, Quote> quotes = _shareQuoteProvider
                .GetQuotes(watchlist.Symbols);

            List<WatchlistShare> shares = watchlist.Symbols
                .Where(symbol => shareDetails.ContainsKey(symbol) && quotes.ContainsKey(symbol))
                .Select(symbol => new
                {
                    Symbol = symbol,
                    Detail = shareDetails[symbol],
                    Quote = quotes[symbol],
                })
                .Select(i => new WatchlistShare(i.Symbol, i.Detail.Name, i.Quote.Last, i.Quote.Change, i.Quote.ChangePercent))
                .ToList();

            return new WatchlistDetails(watchlist.Id, watchlist.Name, shares);
        }

        /// <summary>
        /// Opens the new watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the watchlist.</param>
        /// <param name="name">The watchlist name.</param>
        /// <param name="symbols">The initial symbols on the watchlist.</param>
        /// <returns>The identifier of the newly created watchlist.</returns>
        public Guid CreateWatchlist(Guid userId, string name, IEnumerable<string> symbols)
        {
            Watchlist watchlist = Watchlist.CreateNew(userId, name, symbols);
            _watchlistRepository.Save(watchlist);
            return watchlist.Id;
        }

        /// <summary>
        /// Rename an existing watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the watchlist.</param>
        /// <param name="watchlistId">The unique identifier of the watchlist to rename.</param>
        /// <param name="name">The watchlist name.</param>
        public void RenameWatchlist(Guid userId, Guid watchlistId, string name)
        {
            Watchlist watchlist = GetWatchlist(userId, watchlistId);
            watchlist.RenameWatchlist(name);
            _watchlistRepository.Save(watchlist);
        }

        /// <summary>
        /// Delete an existing watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the watchlist.</param>
        /// <param name="watchlistId">The unique identifier of the watchlist to delete.</param>
        public void DeleteWatchlist(Guid userId, Guid watchlistId)
        {
            Watchlist watchlist = GetWatchlist(userId, watchlistId);
            _watchlistRepository.Delete(watchlist);
        }

        /// <summary>
        /// Adds the supplied share to the watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the watchlist.</param>
        /// <param name="watchlistId">The unique identifier of the watchlist.</param>
        /// <param name="symbol">The share symbol.</param>
        public void AddShare(Guid userId, Guid watchlistId, string symbol)
        {
            Watchlist watchlist = GetWatchlist(userId, watchlistId);
            watchlist.AddShare(symbol);
            _watchlistRepository.Save(watchlist);
        }

        public void RemoveShare(Guid userId, Guid watchlistId, string symbol)
        {
            Watchlist watchlist = GetWatchlist(userId, watchlistId);
            watchlist.RemoveShare(symbol);
            _watchlistRepository.Save(watchlist);
        }

        /// <summary>
        /// Removes the supplied share from the watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the watchlist.</param>
        /// <param name="watchlistId">The unique identifier of the watchlist.</param>
        /// <param name="symbol">The share symbol.</param>
        private Watchlist GetWatchlist(Guid userId, Guid watchlistId)
        {
            Watchlist watchlist = _watchlistRepository.GetById(watchlistId);

            if (watchlist == null || watchlist.UserId != userId)
            {
                throw new EntityNotFoundException(nameof(Watchlist), watchlistId);
            }

            return watchlist;
        }
    }
}
