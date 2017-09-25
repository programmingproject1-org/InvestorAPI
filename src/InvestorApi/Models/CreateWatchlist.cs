using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Models
{
    /// <summary>
    /// Command to create a new watchlist.
    /// </summary>
    public class CreateWatchlist
    {
        /// <summary>
        /// Gets or sets the watchlist name.
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
