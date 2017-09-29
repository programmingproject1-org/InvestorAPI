using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.Entities
{
    public class Watchlist
    {
        private Watchlist()
        {
            // Required for instantiation by repository.
        }

        private Watchlist(Guid id, Guid userId, string name, IEnumerable<string> symbols)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Symbols = symbols?.ToArray() ?? new string[0];
        }

        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public string Name { get; private set; }

        public string[] Symbols { get; private set; }

        public static Watchlist CreateNew(Guid userId, string name, IEnumerable<string> symbols)
        {
            return new Watchlist(Guid.NewGuid(), userId, name, symbols);
        }

        public void RenameWatchlist(string name)
        {
            Name = name;
        }

        public void AddShare(string symbol)
        {
            if (Symbols.Any(s => s == symbol))
            {
                return;
            }

            var symbols = Symbols.ToList() ?? new List<string>();
            symbols.Add(symbol);
            Symbols = symbols.ToArray();
        }

        public void RemoveShare(string symbol)
        {
            var symbols = Symbols.ToList();
            symbols.Remove(symbol);
            Symbols = symbols.ToArray();
        }

        internal WatchlistInfo ToWatchlistInfo()
        {
            return new WatchlistInfo(Id, Name);
        }
    }
}
