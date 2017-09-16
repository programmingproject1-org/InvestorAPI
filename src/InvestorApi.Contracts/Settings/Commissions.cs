using System.Collections.Generic;

namespace InvestorApi.Contracts.Settings
{
    /// <summary>
    /// Specifies the entire commission table for an order.
    /// </summary>
    public class Commissions
    {
        /// <summary>
        /// Gets or sets the fixed commission ranges.
        /// </summary>
        public ICollection<CommissionRange> Fixed { get; set; }

        /// <summary>
        /// Gets or sets the percentage commission ranges.
        /// </summary>
        public ICollection<CommissionRange> Percentage { get; set; }
    }
}
