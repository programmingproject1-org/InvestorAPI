using InvestorApi.Contracts.Dtos;
using InvestorApi.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.Entities
{
    /// <summary>
    /// Encapsulates the domain logic for a watchlist.
    /// </summary>
    public class Watchlist
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="Watchlist"/> class from being created.
        /// </summary>
        /// <remarks>
        /// This default constructor must not be removed!
        /// It is required by the repository to instantiate new instances of the class.
        /// </remarks>
        private Watchlist()
        {
            // Required for instantiation by repository.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Watchlist"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the watchlist.</param>
        /// <param name="userId">The unique identifier  of the user who owns the watchlist.</param>
        /// <param name="name">The name of the watchlist.</param>
        /// <param name="symbols">The share symbols on the watchlist.</param>
        private Watchlist(Guid id, Guid userId, string name, IEnumerable<string> symbols)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Symbols = symbols?.ToArray() ?? new string[0];
        }

        /// <summary>
        /// Gets the unique identifier of the watchlist.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the unique identifier  of the user who owns the watchlist.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the name of the watchlist.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the share symbols on the watchlist.
        /// </summary>
        public string[] Symbols { get; private set; }

        /// <summary>
        /// Creates the new watchlist.
        /// </summary>
        /// <param name="userId">The unique identifier  of the user who owns the watchlist.</param>
        /// <param name="name">The name of the watchlist.</param>
        /// <param name="symbols">The share symbols on the watchlist.</param>
        /// <returns>The newly created watchlist.</returns>
        public static Watchlist CreateNew(Guid userId, string name, IEnumerable<string> symbols)
        {
            Validate.NotEmpty(userId, nameof(userId));
            Validate.NotNullOrWhitespace(name, nameof(name));

            return new Watchlist(Guid.NewGuid(), userId, name, symbols);
        }

        /// <summary>
        /// Renames the watchlist.
        /// </summary>
        /// <param name="name">The new name of the watchlist.</param>
        public void RenameWatchlist(string name)
        {
            Validate.NotNullOrWhitespace(name, nameof(name));

            Name = name;
        }

        /// <summary>
        /// Adds a share to the watchlist.
        /// </summary>
        /// <param name="symbol">The symbol of the share to add.</param>
        public void AddShare(string symbol)
        {
            Validate.NotNullOrWhitespace(symbol, nameof(symbol));

            if (Symbols.Any(s => s == symbol))
            {
                return;
            }

            var symbols = Symbols.ToList() ?? new List<string>();
            symbols.Add(symbol);
            Symbols = symbols.ToArray();
        }

        /// <summary>
        /// Removes a share from the watchlist.
        /// </summary>
        /// <param name="symbol">The symbol of the share to remove.</param>
        public void RemoveShare(string symbol)
        {
            Validate.NotNullOrWhitespace(symbol, nameof(symbol));

            var symbols = Symbols.ToList();
            symbols.Remove(symbol);
            Symbols = symbols.ToArray();
        }

        /// <summary>
        /// Exports the state of the entity to a new instance of <see cref="WatchlistInfo"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="WatchlistInfo"/> with the current state of the entity.</returns>
        internal WatchlistInfo ToWatchlistInfo()
        {
            return new WatchlistInfo(Id, Name);
        }
    }
}
