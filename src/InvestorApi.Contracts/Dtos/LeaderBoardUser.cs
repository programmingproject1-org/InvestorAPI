namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// A single user on the leader board.
    /// </summary>
    public class LeaderBoardUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderBoardUser"/> class.
        /// </summary>
        /// <param name="isCurrentUser">A value indicating whether the user is the current user.</param>
        /// <param name="rank">The dequential rank.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="gravatarUrl">The user's gravata URL.</param>
        /// <param name="totalAccountValue">The total account value.</param>
        /// <param name="profit">The total profit.</param>
        /// <param name="profitPercent">The total profit in percent.</param>
        public LeaderBoardUser(int rank, bool isCurrentUser, string displayName, string gravatarUrl,
            decimal totalAccountValue, decimal profit, decimal profitPercent)
        {
            Rank = rank;
            IsCurrentUser = isCurrentUser;
            DisplayName = displayName;
            GravatarUrl = gravatarUrl;
            TotalAccountValue = totalAccountValue;
            Profit = profit;
            ProfitPercent = profitPercent;
        }

        /// <summary>
        /// Gets or sets the current rank.
        /// </summary>
        public int Rank { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the user is the current user.
        /// </summary>
        public bool IsCurrentUser { get; private set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets the user's gravatar URL.
        /// </summary>
        public string GravatarUrl { get; private set; }

        /// <summary>
        /// Gets the total account value.
        /// </summary>
        public decimal TotalAccountValue { get; private set; }

        /// <summary>
        /// Gets the total profit.
        /// </summary>
        public decimal Profit { get; private set; }

        /// <summary>
        /// Gets the total profit in percent.
        /// </summary>
        public decimal ProfitPercent { get; private set; }
    }
}
