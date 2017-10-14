using System;

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
        /// <param name="displayName">The display name.</param>
        /// <param name="totalAccountValue">The total account value.</param>
        /// <param name="profit">The total profit.</param>
        /// <param name="profitPercent">The total profit in percent.</param>
        /// <param name="isCurrentUser">A value indicating whether the user is the current user.</param>
        public LeaderBoardUser(string displayName, decimal totalAccountValue, decimal profit, decimal profitPercent, bool isCurrentUser)
        {
            DisplayName = displayName;
            TotalAccountValue = totalAccountValue;
            Profit = profit;
            ProfitPercent = profitPercent;
            IsCurrentUser = isCurrentUser;
        }

        /// <summary>
        /// Gets or sets the current rank.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName { get; private set; }

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

        /// <summary>
        /// Gets a value indicating whether the user is the current user.
        /// </summary>
        public bool IsCurrentUser { get; set; }
    }
}
