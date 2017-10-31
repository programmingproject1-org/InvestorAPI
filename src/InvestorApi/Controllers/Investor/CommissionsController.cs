using InvestorApi.Contracts;
using InvestorApi.Contracts.Settings;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InvestorApi.Controllers.Investor
{
    /// <summary>
    /// The API controller to return information about applicable commissions.
    /// </summary>
    [Route("api/1.0/commissions")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.InvestorsGroup)]
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
        /// The API operation enables users to retrieve a table which lists all applicable commissions for an executed buy order.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <returns>The access token response.</returns>
        [HttpGet("buy")]
        [Authorize]
        [SwaggerResponse(200, Description = "Success.", Type = typeof(Commissions))]
        public IActionResult GetBuyCommissions()
        {
            Commissions commissions = _settingService.GetBuyCommissions();
            return Ok(commissions);
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
            Commissions commissions = _settingService.GetSellCommissions();
            return Ok(commissions);
        }
    }
}
