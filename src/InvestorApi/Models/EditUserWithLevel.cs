using InvestorApi.Contracts;
using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Models
{
    /// <summary>
    /// Command to edit an existing user.
    /// </summary>
    public class EditUserWithLevel
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [MinLength(5)]
        [MaxLength(30)]
        [RegularExpression(ValidationRegularExpressions.AlphaNumeric)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [RegularExpression(ValidationRegularExpressions.EmailAddress, ErrorMessage = "The email address is invalid.")]
        [MaxLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user level.
        /// </summary>
        public UserLevel? Level { get; set; }
    }
}
