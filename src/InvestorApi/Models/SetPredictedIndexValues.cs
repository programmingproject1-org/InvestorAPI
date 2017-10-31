using System.ComponentModel.DataAnnotations;

namespace InvestorApi.Models
{
    /// <summary>
    /// The model to set preditected index values.
    /// </summary>
    public class SetPredictedIndexValues
    {
        /// <summary>
        /// Gets or sets the predicted index value in one day.
        /// </summary>
        public decimal? ValueInOneDay { get; set; }

        /// <summary>
        /// Gets or sets the predicted index value in one week.
        /// </summary>
        public decimal? ValueInOneWeek { get; set; }
    }
}
