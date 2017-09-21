using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Models
{
    /// <summary>
    /// Command to create a new user.
    /// </summary>
    public class CreateUser
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        [RegularExpression(ValidationRegularExpressions.AlphaNumeric)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required]
        [RegularExpression(ValidationRegularExpressions.EmailAddress, ErrorMessage = "The email address is invalid.")]
        [MaxLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password in clear-text.
        /// </summary>
        [Required]
        [MinLength(8)]
        [MaxLength(30)]
        public string Password { get; set; }
    }
}
