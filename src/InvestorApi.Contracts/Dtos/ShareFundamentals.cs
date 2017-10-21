using System;

namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains fundamental information about a share.
    /// </summary>
    public class ShareFundamentals : ShareSummary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShareFundamentals"/> class.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="name">The display name of the share.</param>
        /// <param name="industry">The industry.</param>
        public ShareFundamentals(string symbol, string name, string industry)
            : base(symbol, name, industry)
        {
        }

        /// <summary>
        /// Gets or sets the market capitalization.
        /// </summary>
        public long? MarketCap { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        public long? Volume { get; set; }

        /// <summary>
        /// Gets or sets the average daily volume.
        /// </summary>
        public long? AverageDailyVolume { get; set; }

        /// <summary>
        /// Gets or sets the previous close.
        /// </summary>
        public decimal? PreviousClose { get; set; }

        /// <summary>
        /// Gets or sets the 52 week low.
        /// </summary>
        public decimal? Low52Weeks { get; set; }

        /// <summary>
        /// Gets or sets the 52 week high.
        /// </summary>
        public decimal? High52Weeks { get; set; }

        /// <summary>
        /// Gets or sets the dividend per share.
        /// </summary>
        public decimal? DividendShare { get; set; }

        /// <summary>
        /// Gets or sets the dividend yield.
        /// </summary>
        public decimal? DividendYield { get; set; }

        /// <summary>
        /// Gets or sets the ex dividend date.
        /// </summary>
        public string ExDividendDate { get; set; }

        /// <summary>
        /// Gets or sets the dividend pay date.
        /// </summary>
        public string DividendPayDate { get; set; }

        /// <summary>
        /// Gets or sets the P/E ratio.
        /// </summary>
        public decimal? PERatio { get; set; }

        /// <summary>
        /// Gets or sets the book value.
        /// </summary>
        public decimal? BookValue { get; set; }

        /// <summary>
        /// Gets or sets the price book ratio.
        /// </summary>
        public decimal? PriceBook { get; set; }

        /// <summary>
        /// Gets or sets the earnings per share.
        /// </summary>
        public decimal? EarningsShare { get; set; }

        /// <summary>
        /// Gets or sets the 200 day moving average.
        /// </summary>
        public decimal? MovingAverage200Days { get; set; }

        /// <summary>
        /// Gets or sets the 50 day moving average.
        /// </summary>
        public decimal? MovingAverage50Days { get; set; }
    }
}
