using InvestorApi.Contracts.Dtos;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// A service to provide share prediction data.
    /// </summary>
    public interface ISharePredictionsProvider
    {
        /// <summary>
        /// Gets the predictions for the share with the supplied symbol.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <returns>The share's predicitions.</returns>
        SharePredictions GetSharePredictions(string symbol);
    }
}
