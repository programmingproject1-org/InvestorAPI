using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Models;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InvestorApi.Controllers.Investor
{
    /// <summary>
    /// The API controller provides share price and detail information.
    /// </summary>
    [Route("api/1.0/shares")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.InvestorsGroup)]
    public class SharesController : Controller
    {
        private static readonly IDictionary<string, string> _intervals = new Dictionary<string, string>()
        {
            ["1d"] = "2m",
            ["5d"] = "15m",
            ["1mo"] = "1h",
            ["3mo"] = "1d",
            ["6mo"] = "1d",
            ["ytd"] = "1d",
            ["1y"] = "1wk",
            ["2y"] = "1wk",
            ["5y"] = "1mo",
            ["10y"] = "1mo",
            ["max"] = "1mo"
        };

        private IShareInfoProvider _shareInfoProvider;
        private IShareQuoteProvider _shareQuoteProvider;
        private IShareFundamentalsProvider _shareFundamentalsProvider;
        private ISharePriceHistoryProvider _sharePriceHistoryProvider;
        private IShareDividendHistoryProvider _shareDividendHistoryProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharesController"/> class.
        /// </summary>
        /// <param name="shareInfoProvider">Injected instance of <see cref="IShareInfoProvider"/>.</param>
        /// <param name="shareQuoteProvider">Injected instance of <see cref="IShareQuoteProvider"/>.</param>
        /// <param name="shareFundamentalsProvider">Injected instance of <see cref="IShareFundamentalsProvider"/>.</param>
        /// <param name="sharePriceHistoryProvider">Injected instance of <see cref="ISharePriceHistoryProvider"/>.</param>
        /// <param name="shareDividendHistoryProvider">Injected instance of <see cref="IShareDividendHistoryProvider"/>.</param>
        public SharesController(
            IShareInfoProvider shareInfoProvider,
            IShareQuoteProvider shareQuoteProvider,
            IShareFundamentalsProvider shareFundamentalsProvider,
            ISharePriceHistoryProvider sharePriceHistoryProvider,
            IShareDividendHistoryProvider shareDividendHistoryProvider)
        {
            _shareInfoProvider = shareInfoProvider;
            _shareQuoteProvider = shareQuoteProvider;
            _shareFundamentalsProvider = shareFundamentalsProvider;
            _sharePriceHistoryProvider = sharePriceHistoryProvider;
            _shareDividendHistoryProvider = shareDividendHistoryProvider;
        }

        /// <summary>
        /// Search for shares.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to search for shares by symbol or name.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageNumber">The page number to return. (Default = 1)</param>
        /// <param name="pageSize">The page size to apply. (Default = 100)</param>
        /// <returns>The action response.</returns>
        [HttpGet("")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(ListResult<ShareInfo>))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        public IActionResult FindShares(
            [FromQuery][Required][MinLength(1)]string searchTerm,
            [FromQuery][Range(1, 1000)]int? pageNumber,
            [FromQuery][Range(1, 100)]int? pageSize)
        {
            var details = _shareInfoProvider.FindShares(searchTerm, null, pageNumber ?? 1, pageSize ?? 100);
            return Ok(details);
        }

        /// <summary>
        /// Get historical prices for a share.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to retrieve historical prices for the share with the supplied symbol.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="symbol">The symbol of the share to return prices for.</param>
        /// <param name="range">The date range. Possible values are: 1d, 5d, 1mo, 3mo, 6mo, 1y, 2y, 5y, 10y, ytd, max</param>
        /// <param name="interval">The interval. Possible values are: 2m, 15m, 1h, 1d, 1wk, 1mo</param>
        /// <param name="endTime">The end time of the period.</param>
        /// <returns>The action response.</returns>
        [HttpGet("{symbol}/prices")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(SharePrice[]))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "Share not found.")]
        public IActionResult GetPrices(
            [FromRoute][MinLength(3)]string symbol,
            [FromQuery][Required]string range,
            [FromQuery]string interval,
            [FromQuery]DateTime? endTime)
        {
            if (string.IsNullOrEmpty(range) || !_intervals.ContainsKey(range))
            {
                throw new ValidationException("Invalid range specified.");
            }

            if (interval != null)
            {
                if (_intervals.Values.All(value => value != interval))
                {
                    throw new ValidationException("Invalid interval specified.");
                }
            }
            else
            {
                interval = _intervals[range];
            }

            var prices = _sharePriceHistoryProvider.GetPriceHistory(symbol, endTime ?? DateTime.UtcNow, range, interval);
            if (prices == null)
            {
                return NotFound();
            }

            var response = new PriceResponse
            {
                Range = range,
                Interval = interval,
                Prices = prices.Select(p => new Price
                {
                    Timestamp = p.Timestamp,
                    Open = p.Open,
                    High = p.High,
                    Low = p.Low,
                    Close = p.Close,
                    Volume = p.Volume
                })
            };

            return Ok(response);
        }

        /// <summary>
        /// Get historical dividends for a share.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to retrieve historical dividends for the share with the supplied symbol.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="symbol">The symbol of the share to return dividends for.</param>
        /// <param name="range">The date range. Possible values are: 1y, 2y, 5y, 10y, max</param>
        /// <returns>The action response.</returns>
        [HttpGet("{symbol}/dividends")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(ShareDividend[]))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "Share not found.")]
        public IActionResult GetDividends(
            [FromRoute][MinLength(3)]string symbol,
            [FromQuery]string range)
        {
            var dividends = _shareDividendHistoryProvider.GetDividendHistory(symbol, range ?? "max");
            if (dividends == null)
            {
                return NotFound();
            }

            return Ok(dividends);
        }

        /// <summary>
        /// Get fundamental data for a share.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to retrieve fundamental data for the share with the supplied symbol.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="symbol">The symbol of the share to return data for.</param>
        /// <returns>The action response.</returns>
        [HttpGet("{symbol}/fundamentals")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(ShareFundamentals))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "Share not found.")]
        public IActionResult GetFundamentals([FromRoute][MinLength(3)]string symbol)
        {
            var fundamentals = _shareFundamentalsProvider.GetShareFundamentals(symbol);
            if (fundamentals == null)
            {
                return NotFound();
            }

            return Ok(fundamentals);
        }

        /// <summary>
        /// Get current quotes for a list of shares.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to retrieve the current quote for the shares with the supplied symbols.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="symbols">The symbols of the share to return, separated by commas.</param>
        /// <returns>The action response.</returns>
        [HttpGet("quotes")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(Quote[]))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        public IActionResult GetQuotes([FromQuery][MinLength(3)]string symbols)
        {
            var items = symbols
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim().ToUpperInvariant())
                .ToArray();

            var quotes = _shareQuoteProvider.GetQuotes(items);
            return Ok(quotes.Values);
        }
    }
}
