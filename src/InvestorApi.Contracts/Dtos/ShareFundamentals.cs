using System;

namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains fundamental information about a share.
    /// </summary>
    public class ShareFundamentals : ShareInfo
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
        /// Gets or sets the previous trading day's close..
        /// </summary>
        public decimal? PreviousClose { get; set; }

        /// <summary>
        /// Gets or sets the ex dividend date.
        /// </summary>
        public DateTime? ExDividendDate { get; set; }

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
        /// Gets or sets the trailing EPS.
        /// </summary>
        public decimal? TrailingEps { get; set; }

        /// <summary>
        /// Gets or sets the forward EPS.
        /// </summary>
        public decimal? ForwardEps { get; set; }

        /// <summary>
        /// Gets or sets the beta.
        /// </summary>
        public decimal? Beta { get; set; }

        /// <summary>
        /// Gets or sets the 52-week change.
        /// </summary>
        public decimal? Change52Weeks { get; set; }

        /// <summary>
        /// Gets or sets the target high price.
        /// </summary>
        public decimal? TargetHighPrice { get; set; }

        /// <summary>
        /// Gets or sets the target low price.
        /// </summary>
        public decimal? TargetLowPrice { get; set; }

        /// <summary>
        /// Gets or sets the target mean price.
        /// </summary>
        public decimal? TargetMeanPrice { get; set; }

        /// <summary>
        /// Gets or sets the target median price.
        /// </summary>
        public decimal? TargetMedianPrice { get; set; }

        /// <summary>
        /// Gets or sets the consensus analyst recommendation.
        /// </summary>
        public string AnalystRecommendation { get; set; }

        /// <summary>
        /// Gets or sets the number of analyst opinions.
        /// </summary>
        public int? NumberOfAnalystOpinions { get; set; }

        /// <summary>
        /// Gets or sets the total revenue.
        /// </summary>
        public long? TotalRevenue { get; set; }

        /// <summary>
        /// Gets or sets the earnings growth.
        /// </summary>
        public decimal? EarningsGrowth { get; set; }

        /// <summary>
        /// Gets or sets the revenue growth.
        /// </summary>
        public decimal? RevenueGrowth { get; set; }

        /// <summary>
        /// Gets or sets the 52 week low.
        /// </summary>
        public decimal? Low52Weeks { get; set; }

        /// <summary>
        /// Gets or sets the 52 week high.
        /// </summary>
        public decimal? High52Weeks { get; set; }

        /// <summary>
        /// Gets or sets the 200 day moving average.
        /// </summary>
        public decimal? MovingAverage200Days { get; set; }

        /// <summary>
        /// Gets or sets the 50 day moving average.
        /// </summary>
        public decimal? MovingAverage50Days { get; set; }

        /// <summary>
        /// Gets or sets the average daily volume.
        /// </summary>
        public long? AverageDailyVolume { get; set; }
    }
}
