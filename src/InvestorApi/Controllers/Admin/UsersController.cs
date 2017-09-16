using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Security;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace InvestorApi.Controllers.Admin
{
    /// <summary>
    /// The API controller provides management operations for users.
    /// </summary>
    [Route("api/1.0/admin/users")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.AdministratorsGroup)]
    public class UsersController : Controller
    {
        private IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">Injected instance of <see cref="IUserService"/>.</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all existing users.
        /// </summary>
        /// <remarks>
        /// The API operation enables administrators to retrieve a list of all existing users.
        /// The caller must provide a valid access token and must be an `Administrator`.
        /// </remarks>
        /// <param name="pageNumber">The page number to return. (Default = 1)</param>
        /// <param name="pageSize">The page size to apply. (Default = 100)</param>
        /// <returns>The action response.</returns>
        [HttpGet("")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(200, Description = "Success", Type = typeof(ListResult<UserInfo>))]
        [SwaggerResponse(401, Description = "User not authenticated")]
        [SwaggerResponse(403, Description = "User not authorized")]
        public IActionResult ListUsers(int? pageNumber, int? pageSize)
        {
            var users = _userService.ListUsers(pageNumber ?? 1, pageSize ?? 100);
            return Ok(users);
        }

        /// <summary>
        /// Delete an existing user.
        /// </summary>
        /// <remarks>
        /// The API operation enables administrators to delete an existing user.
        /// The caller must provide a valid access token and must be an `Administrator`.
        /// </remarks>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>The action response.</returns>
        [HttpDelete("{userId:guid}")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(204, Description = "User successfully deleted")]
        [SwaggerResponse(401, Description = "User not authenticated")]
        [SwaggerResponse(403, Description = "User not authorized")]
        [SwaggerResponse(404, Description = "User not found")]
        public IActionResult DeleteUser([FromRoute]Guid userId)
        {
            _userService.DeleteUser(userId);
            return StatusCode(204);
        }
    }
}
