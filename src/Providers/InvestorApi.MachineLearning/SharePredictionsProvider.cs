using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Contracts.Settings;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.MachineLearning
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
            // Get the last price and the 1-year change of the index.
            var indexPriceAndChange = GetLastPriceAndChange(IndexSymbol);
            if (indexPriceAndChange == null)
            {
                return null;
            }

            // Get the last price and the 1-year change of the share.
            var sharePriceAndChange = GetLastPriceAndChange(symbol);
            if (sharePriceAndChange == null)
            {
                return null;
            }

            // Get the index prediections from the settings.
            // These were previsouly stored by the machine learning script.
            IndexPredictions predictions = _settingService.GetIndexPredictions();

            // Calculate the share's beta value.
            decimal beta = sharePriceAndChange.Item2 * (1 - indexPriceAndChange.Item2);

            // Claculate the predicted index change in 1 day and 1 week.
            decimal indexChange1Day = (predictions.IndexInOneDay - indexPriceAndChange.Item1) / indexPriceAndChange.Item1;
            decimal indexChange1Week = (predictions.IndexInOneWeek - indexPriceAndChange.Item1) / indexPriceAndChange.Item1;

            // Calculate the predicted price change in 1 day and 1 week.
            decimal priceChange1Day = indexChange1Day * beta;
            decimal priceChange1Week = indexChange1Week * beta;

            // Clauclate the predicted price in 1 day and 1 week.
            decimal priceIn1Day = Math.Round(sharePriceAndChange.Item1 * (1 + priceChange1Day), 3);
            decimal priceIn1Week = Math.Round(sharePriceAndChange.Item1 * (1 + priceChange1Week), 3);

            // Return the predicted prices.
            return new SharePredictions(symbol, priceIn1Day, priceIn1Week);
        }

        private Tuple<decimal, decimal> GetLastPriceAndChange(string symbol)
        {
            return _memoryCache.GetOrCreate("Beta:" + symbol, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));

                // Get the historical share prices of the last 12 months.
                IEnumerable<SharePrice> prices = _sharePriceHistoryProvider.GetPriceHistory(symbol, DateTime.UtcNow, "1y", "1d");
                if (prices == null)
                {
                    return null;
                }

                // Calculate the change.
                decimal[] closes = prices.Where(p => p.Close.HasValue).Select(p => p.Close.Value).ToArray();
                decimal first = closes.First();
                decimal last = closes.Last();
                decimal change = (last - first) / first;

                // Return the previous close and the 1-year change.
                return new Tuple<decimal, decimal>(last, change);
            });
        }
    }
}
