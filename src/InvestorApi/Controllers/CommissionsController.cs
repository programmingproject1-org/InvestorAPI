using InvestorApi.Contracts.Settings;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InvestorApi.Controllers
{
    /// <summary>
    /// The API controller to return information about applicable commissions.
    /// </summary>
    [Route("api/1.0/commissions")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.InvestorsGroup)]
    public class CommissionsController : Controller
    {
        /// <summary>
        /// Get the commission table for buy orders.
        /// </summary>
        /// <remarks>
        /// The API operation enables users to retrieve a table which lists all applicable commissions for an executed buy order.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <returns>The access token response.</returns>
        [HttpGet("buy")]
        [Authorize]
        [SwaggerResponse(200, Description = "Success.", Type = typeof(Commissions))]
        public IActionResult GetBuyCommissions()
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Get the commission table for sell orders.
        /// </summary>
        /// <remarks>
        /// The API operation enables users to retrieve a table which lists all applicable commissions for an executed sell order.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <returns>The access token response.</returns>
        [HttpGet("sell")]
        [Authorize]
        [SwaggerResponse(200, Description = "Success.", Type = typeof(Commissions))]
        public IActionResult GetSellCommissions()
        {
            return StatusCode(501);
        }
    }
}
