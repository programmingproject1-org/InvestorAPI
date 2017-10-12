using InvestorApi.Contracts;
using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Models
{
    /// <summary>
    /// Command to edit an existing user.
    /// </summary>
    public class EditUser
    {
        /// <summary>
        /// Gets or sets the user level.
        /// </summary>
        [Required]
        public UserLevel Level { get; set; }
    }
}
