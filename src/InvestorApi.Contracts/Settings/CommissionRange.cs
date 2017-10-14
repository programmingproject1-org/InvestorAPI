namespace InvestorApi.Contracts.Settings
{
    /// <summary>
    /// Specifies a specific commission range.
    /// </summary>
    public class CommissionRange
    {
        /// <summary>
        /// Gets or sets the lower bound of the range.
        /// </summary>
        public long Min { get; set; }

        /// <summary>
        /// Gets or sets the upper bound of the range.
        /// </summary>
        public long Max { get; set; }

        /// <summary>
        /// Gets or sets the commission value.
        /// </summary>
        public decimal Value { get; set; }
    }
}
