using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace InvestorApi.Extensions
{
    internal static class HttpRequestExtensions
    {
        public static Guid GetUserId(this HttpRequest request)
        {
            string userId = request.HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            return new Guid(userId);
        }
    }
}
