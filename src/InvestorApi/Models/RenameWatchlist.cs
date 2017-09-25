using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Models
{
    /// <summary>
    /// Command to create rename an existing watchlist.
    /// </summary>
    public class RenameWatchlist
    {
        /// <summary>
        /// Gets or sets the watchlist name.
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
