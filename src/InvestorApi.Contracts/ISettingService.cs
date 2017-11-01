using InvestorApi.Contracts.Settings;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// A service to provide access to system settings.
    /// </summary>
    public interface ISettingService
    {
        /// <summary>
        /// Gets the index prediction settings.
        /// </summary>
        /// <returns>The prediction settings.</returns>
        IndexPredictions GetIndexPredictions();

        /// <summary>
        /// Saves the index prediction settings.
        /// </summary>
        /// <param name="settings">The prediction settings.</param>
        void SaveIndexPredictions(IndexPredictions settings);

        /// <summary>
        /// Gets the default account settings.
        /// </summary>
        /// <returns>The settings.</returns>
        DefaultAccountSettings GetDefaultAccountSettings();

        /// <summary>
        /// Saves the default account settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        void SaveDefaultAccountSettings(DefaultAccountSettings settings);

        /// <summary>
        /// Gets the buy commissions.
        /// </summary>
        /// <returns>The commissions.</returns>
        Commissions GetBuyCommissions();

        /// <summary>
        /// Saves the buy commissions.
        /// </summary>
        /// <param name="commissions">The commissions.</param>
        void SaveBuyCommissions(Commissions commissions);

        /// <summary>
        /// Gets the sell commissions.
        /// </summary>
        /// <returns>The commissions.</returns>
        Commissions GetSellCommissions();

        /// <summary>
        /// Saves the sell commissions.
        /// </summary>
        /// <param name="commissions">The commissions.</param>
        void SaveSellCommissions(Commissions commissions);
    }
}
