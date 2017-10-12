using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Models
{
    /// <summary>
    /// Command to place an order.
    /// </summary>
    public class PlaceOrder
    {
        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        [Required]
        public OrderSide Side { get; set; }

        /// <summary>
        /// Gets or sets the symbol of the share.
        /// </summary>
        [Required]
        [MinLength(3)]
        [RegularExpression(ValidationRegularExpressions.Symbol)]
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        [Required]
        [Range(1, 100000000)]
        public long Quantity { get; set; }

        /// <summary>
        /// Gets or sets a unique number used to detect suplicate messages.
        /// </summary>
        [Required]
        public long Nonce { get; set; }
    }
}
