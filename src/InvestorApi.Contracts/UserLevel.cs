namespace InvestorApi.Contracts
{
    /// <summary>
    /// Specified the level of a user.
    /// </summary>
    public enum UserLevel
    {
        /// <summary>
        /// The user is just a friend invited by another user.
        /// </summary>
        Friend = 0,

        /// <summary>
        /// The user is an investor (player).
        /// </summary>
        Investor = 1,

        /// <summary>
        /// The user has administrative privilegues.
        /// </summary>
        Administrator = 2
    }
}
