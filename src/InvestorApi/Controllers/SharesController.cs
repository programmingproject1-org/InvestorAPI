using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InvestorApi.Controllers
{
    /// <summary>
    /// The API controller provides share price and detail information.
    /// </summary>
    [Route("api/1.0/shares")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.InvestorsGroup)]
    public class SharesController : Controller
    {
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
        public IActionResult FindShares([FromQuery]string searchTerm, [FromQuery]int? pageNumber, [FromQuery]int? pageSize)
        {
            return StatusCode(501);
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
        [HttpGet("{symbol}/quote")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(Quote))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "Share not found.")]
        public IActionResult GetQuote([FromRoute]string symbol)
        {
            return StatusCode(501);
        }
    }
}
