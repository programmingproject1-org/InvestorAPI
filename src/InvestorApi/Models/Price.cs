using Newtonsoft.Json;
using System;

namespace InvestorApi.Models
{
    /// <summary>
    /// S historical share price.
    /// </summary>
    public class Price
    {
        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        [JsonProperty("t")]
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets the opening price.
        /// </summary>
        [JsonProperty("o")]
        public decimal Open { get; set; }

        /// <summary>
        /// Gets the highest price in the period.
        /// </summary>
        [JsonProperty("h")]
        public decimal High { get; set; }

        /// <summary>
        /// Gets the lowest price in the period.
        /// </summary>
        [JsonProperty("l")]
        public decimal Low { get; set; }

        /// <summary>
        /// Gets the closing price.
        /// </summary>
        [JsonProperty("c")]
        public decimal Close { get; set; }

        /// <summary>
        /// Gets the traded volume.
        /// </summary>
        [JsonProperty("v")]
        public long Volume { get; set; }
    }
}
