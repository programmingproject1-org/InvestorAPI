using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Models;
using InvestorApi.Security;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InvestorApi.Controllers
{
    /// <summary>
    /// The API controller to issue JWT access tokens.
    /// </summary>
    [Route("api/1.0/token")]
    public class TokenController : Controller
    {
        private IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenController"/> class.
        /// </summary>
        /// <param name="userService">Injected instance of <see cref="IUserService"/>.</param>
        public TokenController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authenticate to get an access token.
        /// </summary>
        /// <remarks>
        /// The API operation enables users to authenticate with their email address and password in order to obtain
        /// a JWT access token which is required to access other API operations.
        /// </remarks>
        /// <param name="body">The access token request claims.</param>
        /// <returns>The access token response.</returns>
        [HttpPost]
        [SwaggerResponse(200, Description = "Access token successfully issued.", Type = typeof(LoginResponse))]
        [SwaggerResponse(400, Description = "The request failed validation.")]
        [SwaggerResponse(401, Description = "Authentication failure.")]
        public IActionResult Login([FromBody]Login body)
        {
            UserInfo result = _userService.Login(body.Email, body.Password);

            if (result == null)
            {
                return StatusCode(401);
            }

            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, result.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, result.DisplayName),
                new Claim(JwtRegisteredClaimNames.Email, result.Email)
            };

            JwtSecurityToken jwt = new JwtSecurityToken(
                JwtSettings.Issuer,
                result.Level == UserLevel.Administrator ? JwtSettings.AdministratorAudience : JwtSettings.InvestorAudience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.Add(JwtSettings.Expiration),
                JwtSettings.SigningCredentials);

            return Ok(new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                Expires = (int)JwtSettings.Expiration.TotalSeconds
            });
        }
    }
}
