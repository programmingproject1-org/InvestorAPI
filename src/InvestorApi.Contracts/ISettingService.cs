using InvestorApi.Contracts.Settings;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// A service to provide access to system settings.
    /// </summary>
    public interface ISettingService
    {
        /// <summary>
        /// Gets the default account settings.
        /// </summary>
        /// <returns></returns>
        DefaultAccountSettings GetDefaultAccountSettings();

        /// <summary>
        /// Saves the default account settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        void SaveDefaultAccountSettings(DefaultAccountSettings settings);

        /// <summary>
        /// Gets the buy commissions.
        /// </summary>
        /// <returns></returns>
        Commissions GetBuyCommissions();

        /// <summary>
        /// Saves the buy commissions.
        /// </summary>
        /// <param name="commissions">The commissions.</param>
        void SaveBuyCommissions(Commissions commissions);

        /// <summary>
        /// Gets the sell commissions.
        /// </summary>
        /// <returns></returns>
        Commissions GetSellCommissions();

        /// <summary>
        /// Saves the sell commissions.
        /// </summary>
        /// <param name="commissions">The commissions.</param>
        void SaveSellCommissions(Commissions commissions);
    }
}
