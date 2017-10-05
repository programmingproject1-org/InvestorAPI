using InvestorApi.Contracts.Dtos;
using System.Collections.Generic;

namespace InvestorApi.Models
{
    /// <summary>
    /// The response of a share price request.
    /// </summary>
    public class PriceResponse
    {
        /// <summary>
        /// Gets or sets the time range.
        /// </summary>
        public string Range { get; set; }

        /// <summary>
        /// Gets or sets the time interval.
        /// </summary>
        public string Interval { get; set; }

        /// <summary>
        /// Gets or sets the prices.
        /// </summary>
        public IEnumerable<Price> Prices { get; set; }
    }
}
