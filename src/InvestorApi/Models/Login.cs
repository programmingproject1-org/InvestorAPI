using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Models
{
    /// <summary>
    /// Command to login a user.
    /// </summary>
    public class Login
    {
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
