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
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password in clear-text.
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
