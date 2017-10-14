using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Models;
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
        /// Get user profile.
        /// </summary>
        /// <remarks>
        /// The API operation enables administrators to retrieve the details of a specifc user.
        /// The caller must provide a valid access token and must be an `Administrator`.
        /// </remarks>
        /// <param name="userId">The unique identifier of the user to edit.</param>
        /// <returns>The action response.</returns>
        [HttpGet("{userId:guid}")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(200, Description = "Success", Type = typeof(UserInfo))]
        [SwaggerResponse(401, Description = "Authorization failed")]
        [SwaggerResponse(403, Description = "User not authorized")]
        [SwaggerResponse(404, Description = "User not found")]
        public IActionResult GetUser([FromRoute]Guid userId)
        {
            var user = _userService.GetUserInfo(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Edit an existing user.
        /// </summary>
        /// <remarks>
        /// The API operation enables administrators to edit an existing user. All fields are optional and only provided fields will be updated.
        /// The caller must provide a valid access token and must be an `Administrator`.
        /// </remarks>
        /// <param name="userId">The unique identifier of the user to edit.</param>
        /// <param name="body">The request body.</param>
        /// <returns>The action response.</returns>
        [HttpPut("{userId:guid}")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(204, Description = "User successfully updated")]
        [SwaggerResponse(400, Description = "Invalid request")]
        [SwaggerResponse(401, Description = "Authorization failed")]
        [SwaggerResponse(403, Description = "User not authorized")]
        [SwaggerResponse(404, Description = "User not found")]
        public IActionResult EditUser([FromRoute]Guid userId, [FromBody]EditUser body)
        {
            _userService.EditUser(userId, body.DisplayName, body.Email, body.Level);
            return NoContent();
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
            return NoContent();
        }
    }
}
