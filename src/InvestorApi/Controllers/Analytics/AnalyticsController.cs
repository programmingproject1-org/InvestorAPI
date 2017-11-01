using InvestorApi.Contracts;
using InvestorApi.Models;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InvestorApi.Controllers.Analytics
{
    /// <summary>
    /// The API controller provides share price and detail information.
    /// </summary>
    [Route("api/1.0/analytics")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.AnalyticsGroup)]
    public class AnalyticsController : Controller
    {
        private static readonly IDictionary<string, string> _intervals = new Dictionary<string, string>()
        {
            ["6mo"] = "1d",
            ["1y"] = "1wk",
            ["2y"] = "1wk"
        };

        private ISettingService _settingService;
        private ISharePriceHistoryProvider _sharePriceHistoryProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsController"/> class.
        /// </summary>
        /// <param name="settingService">Injected instance of <see cref="ISettingService"/>.</param>
        /// <param name="sharePriceHistoryProvider">Injected instance of <see cref="ISharePriceHistoryProvider"/>.</param>
        public AnalyticsController(ISettingService settingService, ISharePriceHistoryProvider sharePriceHistoryProvider)
        {
            _settingService = settingService;
            _sharePriceHistoryProvider = sharePriceHistoryProvider;
        }

        /// <summary>
        /// Set the predicted index values.
        /// </summary>
        /// <remarks>
        /// The API operation enables the machine learning service to set the predicted index values.
        /// </remarks>
        /// <param name="body">The request body.</param>
        /// <returns>The action response.</returns>
        [HttpPut("predictions")]
        [SwaggerResponse(204)]
        public IActionResult SetPredictedIndexValues([FromBody]SetPredictedIndexValues body)
        {
            var predictions = _settingService.GetIndexPredictions();

            predictions.IndexInOneDay = body.ValueInOneDay ?? predictions.IndexInOneDay;
            predictions.IndexInOneWeek = body.ValueInOneWeek ?? predictions.IndexInOneWeek;

            _settingService.SaveIndexPredictions(predictions);

            return NoContent();
        }

        /// <summary>
        /// Get historical prices for a share.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to retrieve historical prices for the share with the supplied symbol.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="symbol">The symbol of the share to return prices for.</param>
        /// <param name="range">The date range. Possible values are: 6mo, 1y, 2y</param>
        /// <param name="interval">The interval. Possible values are: 1d, 1wk</param>
        /// <returns>The action response.</returns>
        [HttpGet("shares/{symbol}/prices")]
        [SwaggerResponse(200, Type = typeof(decimal[]))]
        [SwaggerResponse(404, Description = "Share not found.")]
        public IActionResult GetPrices(
            [FromRoute][MinLength(3)]string symbol,
            [FromQuery][Required]string range,
            [FromQuery]string interval)
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

            var prices = _sharePriceHistoryProvider.GetPriceHistory(symbol, DateTime.UtcNow, range, interval);
            if (prices == null)
            {
                return NotFound();
            }

            var response = prices.Select(price => price.Close).ToArray();
            return Ok(response);
        }
    }
}
