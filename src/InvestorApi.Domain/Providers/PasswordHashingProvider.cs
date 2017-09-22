using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InvestorApi.Domain.Providers
{
    /// <summary>
    /// Provides helper methods to securely hash passwords.
    /// </summary>
    internal class PasswordHashingProvider
    {
        private const string HashAlgorithm = "SHA256";
        private const int MinSecretLength = 8;
        private const int MinSaltLength = 8;
        private const int DefaultSaltLength = 20;

        /// <summary>
        /// Computes the hash of the supplied secret.
        /// </summary>
        /// <param name="secret">The plain text secret.</param>
        /// <returns>The hased secret.</returns>
        public string ComputeHash(string secret)
        {
            // Calculate a salt to prevent rainbow table attacks.
            byte[] salt = GenerateSalt(DefaultSaltLength);

            // Now hash the secret together with the salt.
            return ComputeHash(Encoding.UTF8.GetBytes(secret), salt);
        }

        /// <summary>
        /// Computes the hash of the supplied secret and salt.
        /// </summary>
        /// <param name="secret">The plain text secret.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The hashed secret.</returns>
        internal string ComputeHash(byte[] secret, byte[] salt)
        {
            if (secret == null || secret.Length < MinSecretLength)
            {
                throw new ArgumentOutOfRangeException($"The provided secret does not meet the minimum length requirement of {MinSecretLength}.");
            }

            if (salt == null || salt.Length < MinSaltLength)
            {
                throw new ArgumentOutOfRangeException($"The provided salt does not meet the minimum length requirement of {MinSaltLength}.");
            }

            using (var hashAlgorithm = SHA256.Create())
            {
                // Calculate the hash.
                byte[] hash = hashAlgorithm.ComputeHash(salt.Concat(secret).ToArray());

                // Return the hash as well as the salt and the used algorithm (for forward compatibility)
                return $"{HashAlgorithm}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
            }
        }

        /// <summary>
        /// Verifies the supplied hash.
        /// </summary>
        /// <param name="secret">The plain text secret.</param>
        /// <param name="hashedSecret">The hashed secret.</param>
        /// <returns>A value indicating whether the hash matches the secret.</returns>
        public bool VerifyHash(string secret, string hashedSecret)
        {
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentException($"Argument '{nameof(secret)}' is required.");
            }

            if (string.IsNullOrEmpty(hashedSecret))
            {
                throw new ArgumentException($"Argument '{nameof(hashedSecret)}' is required.");
            }

            string[] parts = hashedSecret.Split(':');
            if (parts.Length != 3)
            {
                throw new InvalidOperationException("The stored hash is invalid.");
            }

            string computedHash = ComputeHash(Encoding.UTF8.GetBytes(secret), Convert.FromBase64String(parts[1]));
            return hashedSecret == computedHash;
        }

        private byte[] GenerateSalt(int length)
        {
            if (length < MinSaltLength)
            {
                throw new ArgumentException($"The provided length does not meet the minimum salt length requirement of {MinSaltLength}.");
            }

            using (var random = RandomNumberGenerator.Create())
            {
                var salt = new byte[length];
                random.GetBytes(salt);
                return salt;
            }
        }
    }
}
