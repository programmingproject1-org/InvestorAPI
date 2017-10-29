using System;

namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains information about a single dividend.
    /// </summary>
    public class ShareDividend
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShareDividend"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="value">The value.</param>
        public ShareDividend(DateTime date, decimal value)
        {
            Date = date;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public decimal Value { get; set; }
    }
}
