using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace InvestorApi.Asx
{
    /// <summary>
    /// Implements a share finder and information provider using the public ASX company list.
    /// </summary>
    internal class AsxShareInfoProvider : IShareInfoProvider
    {
        private const string Address = "http://www.asx.com.au/asx/research/ASXListedCompanies.csv";

        private static readonly object _syncLock = new object();
        private static IDictionary<string, ShareInfo> _shares = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsxShareInfoProvider"/> class.
        /// </summary>
        public AsxShareInfoProvider()
        {
            // Load the CSV from the ASX website and keep it in memory.
            Load();
        }

        /// <summary>
        /// Returns summary information for the share with the provided symbol.
        /// </summary>
        /// <param name="symbol">The share symbol to retrun the details for.</param>
        /// <returns>The share details.</returns>
        public ShareInfo GetShareInfo(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException($"Argument '{nameof(symbol)}' is required.");
            }

            Load();

            if (_shares.TryGetValue(symbol, out ShareInfo result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Returns summary information for the shares with the provided symbols.
        /// </summary>
        /// <param name="symbols">The share symbols to retrun the details for.</param>
        /// <returns>The share details.</returns>
        public IReadOnlyDictionary<string, ShareInfo> GetShareInfo(IEnumerable<string> symbols)
        {
            Load();

            return symbols
                .Distinct()
                .Select(symbol => GetShareInfo(symbol))
                .ToDictionary(share => share.Symbol);
        }

        /// <summary>
        /// Finds shares by the supplied criteria.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="industry">The industry.</param>
        /// <param name="pageNumber">Gets the page number to return.</param>
        /// <param name="pageSize">Gets the page size to apply.</param>
        /// <returns>The list of shares which match the search criteria.</returns>
        public ListResult<ShareInfo> FindShares(string searchTerm, string industry, int pageNumber, int pageSize)
        {
            Load();

            var symbolMatches = _shares
                .Select(share => share.Value)
                .Where(share => searchTerm == null || share.Symbol.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Where(share => industry == null || share.Industry == industry);

            var nameMatches = _shares
                .Select(share => share.Value)
                .Where(share => searchTerm == null || share.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1)
                .Where(share => industry == null || share.Industry == industry);

            var allMatches = symbolMatches.Union(nameMatches).ToList();

            return new ListResult<ShareInfo>(
                allMatches.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                pageNumber,
                pageSize,
                allMatches.Count);
        }

        /// <summary>
        /// Download the CSV, parse the data and keep it in memory.
        /// </summary>
        private void Load()
        {
            if (_shares != null)
            {
                return;
            }

            lock (_syncLock)
            {
                if (_shares != null)
                {
                    return;
                }

                using (var client = new HttpClient())
                {
                    // Download and parse the CSV.
                    // Note that we need to skip the first two lines because the first contains headers and the second is blank.
                    var csv = client.GetStringAsync(Address).Result;
                    _shares = csv
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Skip(2)
                        .Select(line => ReadCsvLine(line))
                        .Where(line => line != null)
                        .ToDictionary(line => line.Symbol);
                }
            }
        }

        private ShareInfo ReadCsvLine(string line)
        {
            var values = line.Split(',');
            if (values.Length != 3)
            {
                return null;
            }

            var name = values[0].Substring(1, values[0].Length - 2);
            var symbol = values[1];
            var industry = values[2].Substring(1, values[2].Length - 2);

            if (industry == "Not Applic")
            {
                industry = null;
            }

            return new ShareInfo(symbol, name, industry);
        }
    }
}
