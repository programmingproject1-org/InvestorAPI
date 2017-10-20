using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InvestorApi.Domain.Providers
{
    /// <summary>
    /// Provides 
    /// </summary>
    internal static class GravatarProvider
    {
        /// <summary>
        /// Generates the Gravatar image request URL for the supplied email address.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <returns>The Gravatar image URL.</returns>
        public static string GetGravatarUrl(string email)
        {
            return string.Format("http://www.gravatar.com/avatar/{0}.jpg?d=identicon", Md5Encode(email.ToLowerInvariant()));
        }

        /// <summary>
        /// Convertes the supplied text to an MD5 hash in hexadecimal encoding.
        /// </summary>
        /// <param name="text">The text to create the hash from.</param>
        /// <returns>The hashed and hexadecimal encoded text.</returns>
        private static string Md5Encode(string text)
        {
            using (HashAlgorithm md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                return hash.Aggregate(string.Empty, (current, next) => current + next.ToString("X2").ToLowerInvariant());
            }
        }
    }
}
