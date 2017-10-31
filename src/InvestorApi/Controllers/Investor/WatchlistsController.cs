using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Extensions;
using InvestorApi.Models;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace InvestorApi.Controllers.Investor
{
    /// <summary>
    /// The API controller provides management operations for watchlists.
    /// </summary>
    [Route("api/1.0/watchlists")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.InvestorsGroup)]
    public class WatchlistsController : Controller
    {
        private IWatchlistService _watchlistsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatchlistsController"/> class.
        /// </summary>
        /// <param name="watchlistService">Injected instance of <see cref="IWatchlistService"/>.</param>
        public WatchlistsController(IWatchlistService watchlistService)
        {
            _watchlistsService = watchlistService;
        }

        /// <summary>
        /// Get detailed information about a particular watchlist.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to retrieve detailed information about a particular watchlist.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="watchlistId">The unique identifier of the watchlist to retrieve.</param>
        /// <returns>The action response.</returns>
        [HttpGet("{watchlistId:guid}")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(WatchlistDetails))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "Watchlist not found")]
        public IActionResult GetWatchlistDetails([FromRoute]Guid watchlistId)
        {
            var accounts = _watchlistsService.GetWatchlistDetails(Request.GetUserId(), watchlistId);
            return Ok(accounts);
        }

        /// <summary>
        /// Add a share to a watchlist.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to add a share to an existing watchlist.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="watchlistId">The unique identifier of the watchlist to which the share should be added.</param>
        /// <param name="body">The details of the share to add.</param>
        /// <returns>The action response.</returns>
        [HttpPost("{watchlistId:guid}/shares")]
        [Authorize]
        [SwaggerResponse(201, Description = "Share successfully added to watchlist.")]
        [SwaggerResponse(400, Description = "Request failed validation.")]
        [SwaggerResponse(401, Description = "Authorization failed")]
        public IActionResult AddShareToWatchlist([FromRoute]Guid watchlistId, [FromBody]AddShareToWatchlist body)
        {
            _watchlistsService.AddShare(Request.GetUserId(), watchlistId, body.Symbol);
            return StatusCode(201);
        }

        /// <summary>
        /// Remove a share from a watchlist.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to remove a share from a watchlist.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="watchlistId">The unique identifier of the watchlist from which the share should be removed.</param>
        /// <param name="symbol">The symbol code of the share to remove.</param>
        /// <returns>The action response.</returns>
        [HttpDelete("{watchlistId:guid}/shares/{symbol}")]
        [Authorize]
        [SwaggerResponse(201, Description = "Share successfully removed from watchlist.")]
        [SwaggerResponse(400, Description = "Request failed validation.")]
        [SwaggerResponse(401, Description = "Authorization failed")]
        public IActionResult RemoveShareFromWatchlist([FromRoute]Guid watchlistId, [FromRoute]string symbol)
        {
            _watchlistsService.RemoveShare(Request.GetUserId(), watchlistId, symbol);
            return StatusCode(204);
        }
    }
}
