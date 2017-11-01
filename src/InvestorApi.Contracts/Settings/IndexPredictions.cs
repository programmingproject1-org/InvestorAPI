namespace InvestorApi.Contracts.Settings
{
    /// <summary>
    /// Stores predicted index values.
    /// </summary>
    public class IndexPredictions
    {
        /// <summary>
        /// Gets or sets the predicted index value in one day.
        /// </summary>
        public decimal IndexInOneDay { get; set; }

        /// <summary>
        /// Gets or sets the predicted index value in one week.
        /// </summary>
        public decimal IndexInOneWeek { get; set; }
    }
}
