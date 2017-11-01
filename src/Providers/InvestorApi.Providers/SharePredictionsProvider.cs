using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Contracts.Settings;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Providers
{
    /// <summary>
    /// Implements a share predictions provider using predictions provided by machine learning.
    /// </summary>
    internal class SharePredictionsProvider : ISharePredictionsProvider
    {
        private const string IndexSymbol = "^AXJO";

        private readonly IMemoryCache _memoryCache;
        private readonly ISharePriceHistoryProvider _sharePriceHistoryProvider;
        private readonly ISettingService _settingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePredictionsProvider"/> class.
        /// </summary>
        /// <param name="memoryCache">Injected instance of <see cref="IMemoryCache"/>.</param>
        /// <param name="sharePriceHistoryProvider">Injected instance of <see cref="ISharePriceHistoryProvider"/>.</param>
        /// <param name="settingService">Injected instance of <see cref="ISettingService"/>.</param>
        public SharePredictionsProvider(
            IMemoryCache memoryCache,
            ISharePriceHistoryProvider sharePriceHistoryProvider,
            ISettingService settingService)
        {
            _memoryCache = memoryCache;
            _sharePriceHistoryProvider = sharePriceHistoryProvider;
            _settingService = settingService;
        }

        /// <summary>
        /// Gets the predictions for the share with the supplied symbol.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <returns>The share's predicitions.</returns>
        public SharePredictions GetSharePredictions(string symbol)
        {
            // Get the beta value of the share.
            decimal? beta = GetBeta(symbol);
            if (beta == null)
            {
                return null;
            }

            // Get the index prediections from the settings.
            // These were previsouly stored by the machine learning script.
            IndexPredictions predictions = _settingService.GetIndexPredictions();

            return new SharePredictions(symbol, 0, 0);
        }

        private decimal? GetBeta(string symbol)
        {
            // Get the 12 month change of the share.
            decimal? shareChange = GetChange(symbol);
            if (shareChange == null)
            {
                return null;
            }

            // Get the 12 month change of the index.
            decimal indexChange = GetChange(IndexSymbol).Value;

            // Calculate the beta value for the share.
            return shareChange.Value * (1 - indexChange);
        }

        private decimal? GetChange(string symbol)
        {
            return _memoryCache.GetOrCreate("Beta:" + symbol, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));

                // Get the historical share prices of the last 12 months.
                IEnumerable<SharePrice> prices = _sharePriceHistoryProvider.GetPriceHistory(symbol, DateTime.UtcNow, "1y", "1mo");
                if (prices == null)
                {
                    return (decimal?)null;
                }

                // Calculate the change.
                decimal[] closes = prices.Where(p => p.Close.HasValue).Select(p => p.Close.Value).ToArray();
                decimal first = closes.First();
                decimal last = closes.Last();
                return (last - first) / first;
            });
        }
    }
}
