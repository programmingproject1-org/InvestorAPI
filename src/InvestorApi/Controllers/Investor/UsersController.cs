using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Extensions;
using InvestorApi.Models;
using InvestorApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InvestorApi.Controllers.Investor
{
    /// <summary>
    /// The API controller provides management operations for users.
    /// </summary>
    [Route("api/1.0/users")]
    [ApiExplorerSettings(GroupName = SwaggerConstants.InvestorsGroup)]
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
        /// Get user profile.
        /// </summary>
        /// <remarks>
        /// The API operation enables users to retrieve their own user profile.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <returns>The action response.</returns>
        [HttpGet]
        [Authorize]
        [SwaggerResponse(200, Description = "Success", Type = typeof(UserInfo))]
        [SwaggerResponse(401, Description = "Authentication failed")]
        public IActionResult GetUser()
        {
            var user = _userService.GetUserInfo(Request.GetUserId());

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Create a new user profile.
        /// </summary>
        /// <remarks>
        /// The API operation enables users to register as new user.
        /// The operation does not require an access token. Note that the email address must be unique.
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
        /// Edit an existing user.
        /// </summary>
        /// <remarks>
        /// The API operation enables users to edit their own user profile. All fields are optional and only provided fields will be updated.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <param name="body">The request body.</param>
        /// <returns>The action response.</returns>
        [HttpPut]
        [Authorize]
        [SwaggerResponse(204, Description = "User successfully updated")]
        [SwaggerResponse(400, Description = "Invalid request")]
        [SwaggerResponse(401, Description = "Authentication failed")]
        [SwaggerResponse(404, Description = "User not found")]
        public IActionResult EditUser([FromBody]EditUser body)
        {
            _userService.EditUser(Request.GetUserId(), body.DisplayName, body.Email, null);
            return NoContent();
        }

        /// <summary>
        /// Delete user profile.
        /// </summary>
        /// <remarks>
        /// The API operation enables users to delete their own user profile.
        /// The caller must provide a valid access token.
        /// </remarks>
        /// <returns>The action response.</returns>
        [HttpDelete]
        [Authorize]
        [SwaggerResponse(204, Description = "User successfully deleted")]
        [SwaggerResponse(401, Description = "Authentication failed")]
        public IActionResult DeleteUser()
        {
            _userService.DeleteUser(Request.GetUserId());
            return StatusCode(204);
        }
    }
}
