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
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException"><paramref name="value" /> is <c>null</c>.</exception>
        public static object NotNull(object value, string argumentName)
        {
            ValidateArgumentName(argumentName);

            if (value != null)
            {
                return value;
            }

            var errorMessage = $"The value of '{argumentName}' cannot be null.";
            throw new ValidationException(errorMessage, new ArgumentNullException(errorMessage, argumentName));
        }

        /// <summary>
        /// Enforces that a <see cref="string" /> value is not <c>null</c>, empty or contains only whitespace characters.
        /// </summary>
        /// <param name="value">The argument value to be checked.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        /// <paramref name="value" /> is <c>null</c>, empty or contains only whitespace characters.
        /// </exception>
        public static string NotNullOrWhitespace(string value, string argumentName)
        {
            NotNull(value, argumentName);

            if (value.Length != 0 && !value.All(char.IsWhiteSpace))
            {
                return value;
            }

            var errorMessage =
                $"The string value of '{argumentName}' cannot be empty or contains only whitespace characters.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        /// Enforces that a <see cref="Guid" /> value is not <see cref="Guid.Empty" />.
        /// </summary>
        /// <param name="value">The argument value to be checked.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException"><paramref name="value" /> is <c>Guid.Empty</c></exception>
        public static Guid NotEmpty(Guid value, string argumentName)
        {
            ValidateArgumentName(argumentName);

            if (!Equals(value, Guid.Empty))
            {
                return value;
            }

            var errorMessage = $"The Guid value of '{argumentName}' cannot be Guid.Empty.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        /// Enforces that a value falls within a specified range.
        /// </summary>
        /// <typeparam name="T">The type of the value to compare.</typeparam>
        /// <param name="value">The argument value to be checked.</param>
        /// <param name="minimum">The minimum value of the specified range.</param>
        /// <param name="maximum">The maximum value of the specified range.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        /// <paramref name="value" /> does not fall within the specified range. It is
        /// either less than <paramref name="minimum" /> value or larger than <paramref name="maximum" /> value specified.
        /// </exception>
        public static T Range<T>(T value, T minimum, T maximum, string argumentName)
            where T : IComparable, IComparable<T>
        {
            ValidateArgumentName(argumentName);

            var validator = new RangeAttribute(typeof(T), minimum.ToString(), maximum.ToString());
            if (validator.IsValid(value))
            {
                return value;
            }

            var errorMessage =
                $"The value of '{argumentName}' does not fall within the range of minimum: {minimum} and maximum: {maximum}.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        /// Enforces that a value is not less than a specified minimum.
        /// </summary>
        /// <typeparam name="T">The type of the value to compare.</typeparam>
        /// <param name="value">The argument value to be checked.</param>
        /// <param name="minimum">The minimum value allowed.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        /// <paramref name="value" /> is less than the specified <paramref name="minimum" /> value.
        /// </exception>
        public static T Minimum<T>(T value, T minimum, string argumentName)
            where T : IComparable, IComparable<T>
        {
            ValidateArgumentName(argumentName);

            if (value.CompareTo(minimum) >= 0)
            {
                return value;
            }

            var errorMessage = $"The value of '{argumentName}' cannot be less than {minimum}.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
        }

        /// <summary>
        /// Enforces that a value does not exceed a specified maximum.
        /// </summary>
        /// <typeparam name="T">The type of the value to compare.</typeparam>
        /// <param name="value">The argument value to be checked.</param>
        /// <param name="maximum">The maximum value allowed.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>The value that has been successfully checked.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="argumentName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentName" /> is empty or contains only whitespace characters.</exception>
        /// <exception cref="ValidationException">
        /// <paramref name="value" /> exceeds the specified <paramref name="maximum" /> value.
        /// </exception>
        public static T Maximum<T>(T value, T maximum, string argumentName)
            where T : IComparable, IComparable<T>
        {
            ValidateArgumentName(argumentName);

            if (value.CompareTo(maximum) <= 0)
            {
                return value;
            }

            var errorMessage = $"The value of '{argumentName}' cannot exceed {maximum}.";
            throw new ValidationException(errorMessage, new ArgumentException(errorMessage, argumentName));
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
