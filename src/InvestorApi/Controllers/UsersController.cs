using InvestorApi.Contracts;
using InvestorApi.Models;
using InvestorApi.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace InvestorApi.Controllers
{
    /// <summary>
    /// The API controller provides management operations for users.
    /// </summary>
    [Route("api/1.0/users")]
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
        /// Create a new user.
        /// </summary>
        /// <remarks>
        /// The API operation enables users to register as new user.
        /// The operation does not require an access token. Note that the email address must be unqiue.
        /// </remarks>
        /// <param name="body">The details of the new user.</param>
        /// <returns>The action response.</returns>
        [HttpPost]
        [SwaggerResponse(201, Description = "New user successfully created.")]
        [SwaggerResponse(400, Description = "Request failed validation.")]
        public IActionResult CreateUser([FromBody]CreateUser body)
        {
            _userService.RegisterUser(body.DisplayName, body.Email, body.Password);
            return StatusCode(201);
        }

        /// <summary>
        /// Delete an existing user.
        /// </summary>
        /// <remarks>
        /// The API operation enables administrators to delete an existing user.
        /// The caller must provide a valid access token and must be an `administrator`.
        /// </remarks>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>The action response.</returns>
        [HttpDelete("{userId:guid}")]
        [Authorize(Policy = AuthorizationPolicies.Administrators)]
        [SwaggerResponse(204, Description = "User successfully deleted")]
        [SwaggerResponse(401, Description = "Authorization failed")]
        [SwaggerResponse(404, Description = "User not found")]
        public IActionResult DeleteUser([FromRoute]Guid userId)
        {
            _userService.DeleteUser(userId);
            return StatusCode(204);
        }
    }
}
