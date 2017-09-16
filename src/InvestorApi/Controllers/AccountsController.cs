using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Extensions;
using InvestorApi.Models;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace InvestorApi.Controllers
{
    /// <summary>
    /// The API controller provides management operations for accounts.
    /// </summary>
    [Route("api/1.0/accounts")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.InvestorsGroup)]
    public class AccountsController : Controller
    {
        private IAccountService _accountsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsController"/> class.
        /// </summary>
        /// <param name="accountService">Injected instance of <see cref="IAccountService"/>.</param>
        public AccountsController(IAccountService accountService)
        {
            _accountsService = accountService;
        }

        /// <summary>
        /// Get detailed information about a particular trading account.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to retrieve detailed information about a particular trading account.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="accountId">The unique identifier of the trading account to retrieve.</param>
        /// <returns>The action response.</returns>
        [HttpGet("{accountId:guid}")]
        [Authorize]
        [SwaggerResponse(200, Type = typeof(AccountDetails))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "Account not found")]
        public IActionResult GetAccountDetails([FromRoute]Guid accountId)
        {
            var accounts = _accountsService.GetAccountDetails(Request.GetUserId(), accountId);
            return Ok(accounts);
        }

        /// <summary>
        /// Query account transactions.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to retrieve a list of transactions for the trading account.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="accountId">The unique identifier of the trading account.</param>
        /// <param name="pageNumber">The page number to return. (Default = 1)</param>
        /// <param name="pageSize">The page size to apply. (Default = 100)</param>
        /// <returns>The action response.</returns>
        [HttpGet("{accountId:guid}/transactions")]
        [Authorize]
        [SwaggerResponse(200, Description = "Success", Type = typeof(ListResult<TransactionInfo>))]
        [SwaggerResponse(401, Description = "User not authenticated")]
        [SwaggerResponse(403, Description = "User not authorized")]
        [SwaggerResponse(404, Description = "Account not found")]
        public IActionResult ListTransactions([FromRoute]Guid accountId, [FromQuery]int? pageNumber, [FromQuery]int? pageSize)
        {
            return StatusCode(501);
        }

        /// <summary>
        /// Place a buy or sell order.
        /// </summary>
        /// <remarks>
        /// The API operation enables investors to buy or sell shares by placing an order using the trading account.
        /// Specify either `Buy` or `Sell` for the side.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="accountId">The unique identifier of the trading account.</param>
        /// <param name="order">The order details.</param>
        /// <returns>The action response.</returns>
        [HttpPost("{accountId:guid}/orders")]
        [Authorize]
        [SwaggerResponse(201, Description = "Shares successfully bought.")]
        [SwaggerResponse(400, Description = "Invalid order.")]
        [SwaggerResponse(401, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "Account or share not found")]
        public IActionResult PlaceOrder([FromRoute]Guid accountId, [FromBody]PlaceOrder order)
        {
            return StatusCode(501);
        }
    }
}
