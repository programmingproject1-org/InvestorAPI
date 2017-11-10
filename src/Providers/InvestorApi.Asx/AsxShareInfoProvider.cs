using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace InvestorApi.Asx
{
    /// <summary>
    /// Implements a share finder and information provider using the public ASX company list.
    /// </summary>
    internal class AsxShareInfoProvider : IShareInfoProvider
    {
        private const string ResourcePath = "InvestorApi.Asx.Resources.Shares.json";

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
        /// Download the JSON data and keep it in memory.
        /// </summary>
        private void Load()
        {
            if (_shares != null)
            {
                return;
            }

            lock (_syncLock)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream(ResourcePath))
                using (StreamReader reader = new StreamReader(stream))
                {
                    _shares = JsonConvert
                        .DeserializeObject<ShareInfo[]>(reader.ReadToEnd())
                        .ToDictionary(line => line.Symbol);
                }
            }
        }
    }
}
