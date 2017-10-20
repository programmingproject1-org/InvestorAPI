using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Security;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Controllers.Admin
{
    /// <summary>
    /// The API controller provides management operations for accounts.
    /// </summary>
    [Route("api/1.0/admin/users/{userId:guid}/accounts")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.AdministratorsGroup)]
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
        /// The API operation enables administrators to retrieve detailed information about a particular trading account.
        /// The caller must provide a valid access token and must be an `Administrator`.
        /// </remarks>
        /// <param name="userId">The unique identifier of the user who owns the account.</param>
        /// <param name="accountId">The unique identifier of the trading account to retrieve.</param>
        /// <returns>The action response.</returns>
        [HttpGet("{accountId:guid}")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(200, Type = typeof(AccountDetails))]
        [SwaggerResponse(401, Description = "Authentication failed")]
        [SwaggerResponse(403, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "Account not found")]
        public IActionResult GetAccountDetails([FromRoute]Guid userId, [FromRoute]Guid accountId)
        {
            var accounts = _accountsService.GetAccountDetails(userId, accountId);
            return Ok(accounts);
        }

        /// <summary>
        /// Reset an existing trading account.
        /// </summary>
        /// <remarks>
        /// The API operation enables administrators to reset an existing trading account to its initial state and starting balance.
        /// The caller must provide a valid access token and must be an `Administrator`.
        /// </remarks>
        /// <param name="userId">The unique identifier of the user who owns the account.</param>
        /// <param name="accountId">The unique identifier of the trading account to reset.</param>
        /// <returns>The action response.</returns>
        [HttpPut("{accountId:guid}")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(201, Description = "Account successfully reset.")]
        [SwaggerResponse(401, Description = "Authentication failed")]
        [SwaggerResponse(403, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "Account not found")]
        public IActionResult ResetAccount([FromRoute]Guid userId, [FromRoute]Guid accountId)
        {
            _accountsService.ResetAccount(userId, accountId);
            return StatusCode(201);
        }

        /// <summary>
        /// Query account transactions.
        /// </summary>
        /// <remarks>
        /// The API operation enables administrators to retrieve a list of transactions for a trading account.
        /// The caller must provide a valid access token and must be an `Administrator`.
        /// </remarks>
        /// <param name="userId">The unique identifier of the user who owns the account.</param>
        /// <param name="accountId">The unique identifier of the trading account.</param>
        /// <param name="pageNumber">The page number to return. (Default = 1)</param>
        /// <param name="pageSize">The page size to apply. (Default = 100)</param>
        /// <param name="startDate">The start date of the range to return.</param>
        /// <param name="endDate">The end date of the range to return.</param>
        /// <returns>The action response.</returns>
        [HttpGet("{accountId:guid}/transactions")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(200, Description = "Success", Type = typeof(ListResult<TransactionInfo>))]
        [SwaggerResponse(401, Description = "Authentication failed")]
        [SwaggerResponse(403, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "Account not found")]
        public IActionResult ListTransactions(
            [FromRoute]Guid userId,
            [FromRoute]Guid accountId,
            [FromQuery][Range(1, 1000)]int? pageNumber,
            [FromQuery][Range(1, 100)]int? pageSize,
            [FromQuery]DateTime? startDate,
            [FromQuery]DateTime? endDate)
        {
            var transactions = _accountsService.ListTransactions(userId, accountId, startDate, endDate, pageNumber ?? 1, pageSize ?? 100);
            return Ok(transactions);
        }
    }
}
