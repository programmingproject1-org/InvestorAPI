using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Controllers.Investor
{
    /// <summary>
    /// The API controller provides share price and detail information.
    /// </summary>
    [Route("api/1.0/markets")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.InvestorsGroup)]
    public class MarketsController : Controller
    {
        private IMarketInformationService _marketInformationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharesController"/> class.
        /// </summary>
        /// <param name="marketInformationService">Injected instance of <see cref="IMarketInformationService"/>.</param>
        public MarketsController(IMarketInformationService marketInformationService)
        {
            _marketInformationService = marketInformationService;
        }

        /// <summary>
        /// Get market information.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to get information about a market. Currently only `ASX` is supported.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="symbol">The market symbol.</param>
        /// <returns>The action response.</returns>
        [HttpGet("{symbol}")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(MarketInfo))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        public IActionResult GetMarket([FromRoute][Required][MinLength(3)]string symbol)
        {
            if (!symbol.Equals("ASX", StringComparison.OrdinalIgnoreCase))
            {
                return NotFound();
            }

            var market = _marketInformationService.GetMarket();
            return Ok(market);
        }
    }
}
