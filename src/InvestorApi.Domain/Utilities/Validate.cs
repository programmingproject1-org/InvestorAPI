using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace InvestorApi.Domain.Utilities
{
    /// <summary>
    /// Class with validation utilities to be used in code contract fashion for validating method arguments.
    /// </summary>
    /// <remarks>
    /// Source: https://github.com/ThoughtDesign/Ximo/blob/master/src/Ximo/Validation/Check.cs
    /// </remarks>
    public static class Validate
    {
        /// <summary>
        /// Enforces that an argument value is not null.
        /// </summary>
        /// <param name="value">The argument value to be checked.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException"><paramref name="value" /> is <c>null</c>.</exception>
        public static void NotNull(object value, string argumentName)
        {
            ValidateArgumentName(argumentName);

            if (value != null)
            {
                return;
            }

            throw new ValidationException($"The value of '{argumentName}' cannot be null.");
        }

        /// <summary>
        /// Enforces that a <see cref="string" /> value is not <c>null</c>, empty or contains only whitespace characters.
        /// </summary>
        /// <param name="value">The argument value to be checked.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        /// <paramref name="value" /> is <c>null</c>, empty or contains only whitespace characters.
        /// </exception>
        public static void NotNullOrWhitespace(string value, string argumentName)
        {
            NotNull(value, argumentName);

            if (value.Length != 0 && !value.All(char.IsWhiteSpace))
            {
                return;
            }

            throw new ValidationException($"The string value of '{argumentName}' cannot be empty or contains only whitespace characters.");
        }

        /// <summary>
        /// Enforces that a <see cref="Guid" /> value is not <see cref="Guid.Empty" />.
        /// </summary>
        /// <param name="value">The argument value to be checked.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException"><paramref name="value" /> is <c>Guid.Empty</c></exception>
        public static void NotEmpty(Guid value, string argumentName)
        {
            ValidateArgumentName(argumentName);

            if (!Equals(value, Guid.Empty))
            {
                return;
            }

            throw new ValidationException($"The Guid value of '{argumentName}' cannot be Guid.Empty.");
        }

        /// <summary>
        /// Enforces that a value falls within a specified range.
        /// </summary>
        /// <typeparam name="T">The type of the value to compare.</typeparam>
        /// <param name="value">The argument value to be checked.</param>
        /// <param name="minimum">The minimum value of the specified range.</param>
        /// <param name="maximum">The maximum value of the specified range.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        /// <paramref name="value" /> does not fall within the specified range. It is
        /// either less than <paramref name="minimum" /> value or larger than <paramref name="maximum" /> value specified.
        /// </exception>
        public static void Range<T>(T value, T minimum, T maximum, string argumentName)
            where T : IComparable, IComparable<T>
        {
            ValidateArgumentName(argumentName);

            var validator = new RangeAttribute(typeof(T), minimum.ToString(), maximum.ToString());
            if (validator.IsValid(value))
            {
                return;
            }

            throw new ValidationException($"The value of '{argumentName}' does not fall within the range of minimum: {minimum} and maximum: {maximum}.");
        }

        /// <summary>
        /// Enforces that a value is not less than a specified minimum.
        /// </summary>
        /// <typeparam name="T">The type of the value to compare.</typeparam>
        /// <param name="value">The argument value to be checked.</param>
        /// <param name="minimum">The minimum value allowed.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        /// <paramref name="value" /> is less than the specified <paramref name="minimum" /> value.
        /// </exception>
        public static void Minimum<T>(T value, T minimum, string argumentName)
            where T : IComparable, IComparable<T>
        {
            ValidateArgumentName(argumentName);

            if (value.CompareTo(minimum) >= 0)
            {
                return;
            }

            throw new ValidationException($"The value of '{argumentName}' cannot be less than {minimum}.");
        }

        /// <summary>
        /// Enforces that a value does not exceed a specified maximum.
        /// </summary>
        /// <typeparam name="T">The type of the value to compare.</typeparam>
        /// <param name="value">The argument value to be checked.</param>
        /// <param name="maximum">The maximum value allowed.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        /// <paramref name="value" /> exceeds the specified <paramref name="maximum" /> value.
        /// </exception>
        public static void Maximum<T>(T value, T maximum, string argumentName)
            where T : IComparable, IComparable<T>
        {
            ValidateArgumentName(argumentName);

            if (value.CompareTo(maximum) <= 0)
            {
                return;
            }

            throw new ValidationException($"The value of '{argumentName}' cannot exceed {maximum}.");
        }

        /// <summary>
        /// Enforces that a passwords matches certain strength rules.
        /// </summary>
        /// <param name="value">The password value to validate.</param>
        /// <param name="numeric">The minumum required number of numeric characters.</param>
        /// <param name="lowerCaseCharacters">The minumum required number of lower case characters.</param>
        /// <param name="upperCaseCharacters">The minumum required number of upper case characters.</param>
        /// <param name="specialCharacters">The minumum required number of special characters.</param>
        public static void PasswordStrenth(string value, int numeric, int lowerCaseCharacters, int upperCaseCharacters, int specialCharacters)
        {
            int numericMatches = Regex.Matches(value, "[0-9]").Count;
            int lowerCaseCharacterMatches = Regex.Matches(value, "[a-z]").Count;
            int upperCaseCharacterMatches = Regex.Matches(value, "[A-Z]").Count;
            int specialCharacterMatches = value.Length - numericMatches - lowerCaseCharacterMatches - upperCaseCharacterMatches;

            if ((numericMatches >= numeric) &&
                (lowerCaseCharacterMatches >= lowerCaseCharacters) &&
                (upperCaseCharacterMatches >= upperCaseCharacters) &&
                (specialCharacterMatches >= specialCharacters))
            {
                return;
            }

            throw new ValidationException($"The password must have at least {numeric} numeric, {lowerCaseCharacters} " +
                $"lower case, {upperCaseCharacters} upper case, and {specialCharacters} special character(s).");
        }

        /// <summary>
        /// Validates the name of the argument.
        /// </summary>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        private static void ValidateArgumentName(string argumentName)
        {
            if (argumentName == null)
            {
                var errorMessage = "The argument name cannot be null.";
                throw new ArgumentNullException(nameof(argumentName), errorMessage);
            }

            if (argumentName.Length == 0 || argumentName.All(char.IsWhiteSpace))
            {
                var errorMessage = "The argument name cannot be empty or contains only whitespace characters.";
                throw new ArgumentException(nameof(argumentName), errorMessage);
            }
        }
    }
}
