using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InvestorApi.Security
{
    /// <summary>
    /// Helper class to issue JWT access tokens.
    /// </summary>
    public static class JwtIssuer
    {
        /// <summary>
        /// Issues a JWT access token for the supplied user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="email">The email address.</param>
        /// <param name="isAdministrator">A value indicating whether the user as an administrator.</param>
        /// <returns></returns>
        public static string Issue(Guid userId, string displayName, string email, bool isAdministrator)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, displayName),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };

            JwtSecurityToken jwt = new JwtSecurityToken(
                JwtSettings.Issuer,
                isAdministrator ? JwtSettings.AdministratorAudience : JwtSettings.InvestorAudience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.Add(JwtSettings.Expiration),
                JwtSettings.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
