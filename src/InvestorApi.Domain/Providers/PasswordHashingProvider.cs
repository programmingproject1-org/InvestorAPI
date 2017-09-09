using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InvestorApi.Domain.Providers
{
    internal class PasswordHashingProvider
    {
        private const string HashAlgorithm = "SHA256";
        private const int MinSecretLength = 8;
        private const int MinSaltLength = 8;
        private const int DefaultSaltLength = 20;

        public string ComputeHash(string secret)
        {
            byte[] salt = GenerateSalt(DefaultSaltLength);
            return ComputeHash(Encoding.UTF8.GetBytes(secret), salt);
        }

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
                byte[] hash = hashAlgorithm.ComputeHash(salt.Concat(secret).ToArray());
                return $"{HashAlgorithm}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
            }
        }

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
