using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Models
{
    /// <summary>
    /// Command to add a share to a watchlist.
    /// </summary>
    public class AddShareToWatchlist
    {
        /// <summary>
        /// Gets or sets the symbol code of the share to add.
        /// </summary>
        [Required]
        [MinLength(2)]
        public string Symbol { get; set; }
    }
}
