namespace InvestorApi.Contracts.Settings
{
    /// <summary>
    /// Specifies the default settings for new accounts.
    /// </summary>
    public class DefaultAccountSettings
    {
        /// <summary>
        /// Gets or sets the initial balance of the account.
        /// </summary>
        public decimal InitialBalance { get; set; }
    }
}
