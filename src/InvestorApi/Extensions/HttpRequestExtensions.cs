using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace InvestorApi.Extensions
{
    /// <summary>
    /// Provides commonly used extension methods for the <see cref="HttpRequest"/> class.
    /// </summary>
    internal static class HttpRequestExtensions
    {
        /// <summary>
        /// Gets the identifier of the authenticated user from the HTTP context.
        /// </summary>
        /// <param name="request">The HTTP request context.</param>
        /// <returns>The identifier of the authenticated user.</returns>
        public static Guid GetUserId(this HttpRequest request)
        {
            string userId = request.HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            return new Guid(userId);
        }
    }
}
