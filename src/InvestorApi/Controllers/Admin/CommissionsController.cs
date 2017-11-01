using InvestorApi.Contracts;
using InvestorApi.Contracts.Settings;
using InvestorApi.Security;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InvestorApi.Controllers.Admin
{
    /// <summary>
    /// The API controller to return information about applicable commissions.
    /// </summary>
    [Route("api/1.0/admin/commissions")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.AdministratorsGroup)]
    public class CommissionsController : Controller
    {
        private ISettingService _settingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommissionsController"/> class.
        /// </summary>
        /// <param name="settingService">Injected instance of <see cref="ISettingService"/>.</param>
        public CommissionsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        /// <summary>
        /// Get the commission table for buy orders.
        /// </summary>
        /// <remarks>
        /// The API operation enables administrators to retrieve a table which lists all applicable commissions for an executed buy order.
        /// The caller must provide a valid access token and must be an `Administrator`.
        /// </remarks>
        /// <returns>The access token response.</returns>
        [HttpGet("buy")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(200, Description = "Success.", Type = typeof(Commissions))]
        public IActionResult GetBuyCommissions()
        {
            Commissions commissions = _settingService.GetBuyCommissions();
            return Ok(commissions);
        }


        /// <summary>
        /// Update the commission table for buy orders.
        /// </summary>
        /// <param name="body">The updated commissions.</param>
        /// <remarks>
        /// The API operation enables administrators to update a table which lists all applicable commissions for an executed buy order.
        /// The caller must provide a valid access token and must be an `Administrator`.
        /// </remarks>
        /// <returns>The access token response.</returns>
        [HttpPut("buy")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(204)]
        public IActionResult UpdateBuyCommissions([FromBody]Commissions body)
        {
            _settingService.SaveBuyCommissions(body);
            return NoContent();
        }

        /// <summary>
        /// Get the commission table for sell orders.
        /// </summary>
        /// <remarks>
        /// The API operation enables administrators to retrieve a table which lists all applicable commissions for an executed sell order.
        /// The caller must provide a valid access token and must be an `Administrator`.
        /// </remarks>
        /// <returns>The access token response.</returns>
        [HttpGet("sell")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(200, Description = "Success.", Type = typeof(Commissions))]
        public IActionResult GetSellCommissions()
        {
            Commissions commissions = _settingService.GetSellCommissions();
            return Ok(commissions);
        }

        /// <summary>
        /// Update the commission table for sell orders.
        /// </summary>
        /// <param name="body">The updated commissions.</param>
        /// <remarks>
        /// The API operation enables administrators to update a table which lists all applicable commissions for an executed sell order.
        /// The caller must provide a valid access token and must be an `Administrator`.
        /// </remarks>
        /// <returns>The access token response.</returns>
        [HttpPut("sell")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(204)]
        public IActionResult UpdateSellCommissions([FromBody]Commissions body)
        {
            _settingService.SaveSellCommissions(body);
            return NoContent();
        }
    }
}
