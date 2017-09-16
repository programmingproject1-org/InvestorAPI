using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace InvestorApi.Asx
{
    internal class AsxShareDetailProvider : IShareDetailsProvider
    {
        private const string Address = "http://www.asx.com.au/asx/research/ASXListedCompanies.csv";

        private static readonly object _syncLock = new object();
        private static IDictionary<string, ShareDetails> _shares = null;

        public AsxShareDetailProvider()
        {
            Load();
        }

        public ShareDetails GetShareDetails(string symbol)
        {
            Load();

            if (_shares.TryGetValue(symbol, out ShareDetails result))
            {
                return result;
            }

            return null;
        }

        public IReadOnlyDictionary<string, ShareDetails> GetShareDetails(IEnumerable<string> symbols)
        {
            Load();

            return symbols
                .Select(symbol => GetShareDetails(symbol))
                .ToDictionary(share => share.Symbol);
        }

        public ListResult<ShareDetails> FindShareDetails(string searchTerm, string industry, int pageNumber, int pageSize)
        {
            Load();

            var allResults = _shares
                .Select(share => share.Value)
                .Where(share =>
                    searchTerm == null ||
                    share.Symbol.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    share.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1)
                .Where(share =>
                    industry == null ||
                    share.Industry == industry)
                .ToList();

            return new ListResult<ShareDetails>(
                allResults.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                pageNumber,
                pageSize,
                allResults.Count);
        }

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
                    var csv = client.GetStringAsync(Address).Result;
                    _shares = csv
                        .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                        .Skip(2)
                        .Select(line => ReadCsvLine(line))
                        .Where(line => line != null)
                        .ToDictionary(line => line.Symbol);
                }
            }
        }

        private ShareDetails ReadCsvLine(string line)
        {
            var values = line.Split(',');
            if (values.Length != 3)
            {
                return null;
            }

            var name = values[0].Substring(1, values[0].Length - 2);
            var symbol = values[1];
            var industry = values[2].Substring(1, values[2].Length - 3);

            if (industry == "Not Applic")
            {
                industry = null;
            }

            return new ShareDetails(symbol, name, industry);
        }
    }
}
