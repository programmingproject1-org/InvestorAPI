using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace InvestorApi.Security
{
    internal static class JwtSettings
    {
        public const string Issuer = "InvestorApi";

        public const string InvestorAudience = "Investor";

        public const string AdministratorAudience = "Administrator";

        public static readonly TimeSpan Expiration = TimeSpan.FromDays(7);

        public static readonly byte[] SigningKey = Encoding.ASCII.GetBytes("`6qj+{3fEmd['Z)`~=L7K>gpJ&gh47?-");

        public static readonly SecurityKey SecurtyKey = new SymmetricSecurityKey(SigningKey);

        public static readonly SigningCredentials SigningCredentials = new SigningCredentials(SecurtyKey, SecurityAlgorithms.HmacSha256);
    }
}
