namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains fundamental information about a share.
    /// </summary>
    public class SharePredictions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShareFundamentals"/> class.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="priceInOneDay">The predicted price in one day.</param>
        /// <param name="priceInOneWeek">The predicted price in one week.</param>
        public SharePredictions(string symbol, decimal priceInOneDay, decimal priceInOneWeek)
        {
            PriceInOneDay = priceInOneDay;
            PriceInOneWeek = priceInOneWeek;
        }

        /// <summary>
        /// Gets or sets the predicted price in one day.
        /// </summary>
        public decimal PriceInOneDay { get; set; }

        /// <summary>
        /// Gets or sets the predicted price in one week.
        /// </summary>
        public decimal PriceInOneWeek { get; set; }
    }
}
