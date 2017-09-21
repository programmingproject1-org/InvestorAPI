using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InvestorApi.Controllers
{
    /// <summary>
    /// The API controller provides share price and detail information.
    /// </summary>
    [Route("api/1.0/shares")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.InvestorsGroup)]
    public class SharesController : Controller
    {
        private IShareDetailsProvider _shareDetailsProvider;
        private IShareQuoteProvider _shareQuoteProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharesController"/> class.
        /// </summary>
        /// <param name="shareDetailsProvider">Injected instance of <see cref="IShareDetailsProvider"/>.</param>
        /// <param name="shareQuoteProvider">Injected instance of <see cref="IShareQuoteProvider"/>.</param>
        public SharesController(IShareDetailsProvider shareDetailsProvider, IShareQuoteProvider shareQuoteProvider)
        {
            _shareDetailsProvider = shareDetailsProvider;
            _shareQuoteProvider = shareQuoteProvider;
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
        [SwaggerResponse(200, Type = typeof(ListResult<ShareDetails>))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        public IActionResult FindShares(
            [FromQuery][Required][MinLength(1)]string searchTerm,
            [FromQuery][Range(1, 1000)]int? pageNumber,
            [FromQuery][Range(1, 100)]int? pageSize)
        {
            var details = _shareDetailsProvider.FindShareDetails(searchTerm, null, pageNumber ?? 1, pageSize ?? 100);
            return Ok(details);
        }

        /// <summary>
        /// Get current quote for a share.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to retrieve the current quote for the share with the supplied symbol.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="symbol">The symbol of the share to return.</param>
        /// <returns>The action response.</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{symbol}/quote")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(Quote))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "Share not found.")]
        public IActionResult GetQuote([FromRoute][MinLength(3)]string symbol)
        {
            var quote = _shareQuoteProvider.GetQuote(symbol);

            if (quote == null)
            {
                return NotFound();
            }

            return Ok(quote);
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
