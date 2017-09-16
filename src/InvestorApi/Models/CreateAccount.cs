using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Models
{
    /// <summary>
    /// Command to create a new account.
    /// </summary>
    public class CreateAccount
    {
        /// <summary>
        /// Gets or sets the account name.
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
