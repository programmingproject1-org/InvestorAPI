using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using InvestorApi.Domain.Entities;
using InvestorApi.Domain.Repositories;
using InvestorApi.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.Services
{
    /// <summary>
    /// Provides leader board information.
    /// </summary>
    public class LeaderBoardService : ILeaderBoardService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountService _accountService;
        private readonly ISettingService _settingService;

        private static IList<InternalLeaderBoardUser> _users = new List<InternalLeaderBoardUser>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderBoardService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="accountService">The account service.</param>
        /// <param name="settingService">The setting service.</param>
        public LeaderBoardService(
            IUserRepository userRepository,
            IAccountService accountService,
            ISettingService settingService)
        {
            _userRepository = userRepository;
            _accountService = accountService;
            _settingService = settingService;
        }

        /// <summary>
        /// Loads and calculates the list and keeps it in memory.
        /// </summary>
        public void Load()
        {
            // Get the starting balance assigned to accounts to calculate the profits.
            // Note, if we later decide to change the balance, we need to extend this logic
            // to get the balance from the first account transaction.
            decimal initialBalance = _settingService.GetDefaultAccountSettings().InitialBalance;

            // Get all users.
            var users = _userRepository.ListAllUsersWithAccounts();

            // Calculate the leader board values for each user and order by profit.
            var leaderBoardUsers = users
                .AsParallel()
                .WithDegreeOfParallelism(5)
                .Select(user => GetLeaderBoardUser(user, initialBalance))
                .OrderByDescending(user => user.ProfitPercent)
                .ToList();

            _users = leaderBoardUsers;
        }

        /// <summary>
        /// Gets the leader board users.
        /// </summary>
        /// <param name="currentUserId">The unique identifier of the current user.</param>
        /// <param name="pageNumber">Gets the page number to return.</param>
        /// <param name="pageSize">Gets the page size to apply.</param>
        /// <returns>The leader board users.</returns>
        public ListResult<LeaderBoardUser> GetUsers(Guid currentUserId, int pageNumber, int pageSize)
        {
            Validate.NotEmpty(currentUserId, nameof(currentUserId));
            Validate.Range(pageNumber, 1, 1000, nameof(pageNumber));
            Validate.Range(pageSize, 1, 100, nameof(pageSize));

            var allUsers = GetUsers(currentUserId);
            var users = allUsers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ListResult<LeaderBoardUser>(users, pageNumber, pageSize, allUsers.Count);
        }

        /// <summary>
        /// Gets the leader board users around and including the supplied user.
        /// </summary>
        /// <param name="currentUserId">The unique identifier of the user to get the leader board for.</param>
        /// <param name="neighborCount">The number of neighbors to include</param>
        /// <returns>The leader board users.</returns>
        public IReadOnlyCollection<LeaderBoardUser> GetUsers(Guid currentUserId, int neighborCount)
        {
            Validate.NotEmpty(currentUserId, nameof(currentUserId));
            Validate.Range(neighborCount, 0, 50, nameof(neighborCount));

            var allUsers = GetUsers(currentUserId);

            for (int i = 0; i < allUsers.Count; i++)
            {
                if (allUsers[i].IsCurrentUser)
                {
                    return allUsers
                        .Where(user => user.Rank >= i + 1 - neighborCount)
                        .Where(user => user.Rank <= i + 1 + neighborCount)
                        .ToList();
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the leader board users.
        /// </summary>
        /// <param name="currentUserId">The unique identifier of the current user.</param>
        /// <returns>The leader board users.</returns>
        private IList<LeaderBoardUser> GetUsers(Guid currentUserId)
        {
            int rank = 1;

            return _users
                .Select(user => user.CreateLeaderBoardUser(rank++, user.UserId == currentUserId))
                .ToList();
        }

        /// <summary>
        /// Gets the leader board user from the supplied user.
        /// </summary>
        /// <param name="user">The user to calculate the leader board user from.</param>
        /// <param name="initialBalance">The initial account balance.</param>
        /// <returns>The leader board user.</returns>
        private InternalLeaderBoardUser GetLeaderBoardUser(User user, decimal initialBalance)
        {
            // Caculate the total value of all accounts and get the highest one.
            decimal totalAccountValue = user.Accounts
                .Select(account => (_accountService as AccountService).GetAccountDetails(account))
                .Select(account => account.Positions.Sum(p => p.Quantity * p.LastPrice) + account.Balance)
                .Max();

            // Calculate the profit.
            decimal profit = totalAccountValue - initialBalance;
            decimal profitPercent = profit / initialBalance * 100m;

            // Create the leader board user.
            return new InternalLeaderBoardUser(user.Id, user.DisplayName, user.GravatarUrl, totalAccountValue, profit, profitPercent);
        }

        private class InternalLeaderBoardUser
        {
            public InternalLeaderBoardUser(Guid userId, string displayName, string gravatarUrl,
                decimal totalAccountValue, decimal profit, decimal profitPercent)
            {
                UserId = userId;
                DisplayName = displayName;
                GravatarUrl = gravatarUrl;
                TotalAccountValue = totalAccountValue;
                Profit = profit;
                ProfitPercent = profitPercent;
            }

            public Guid UserId { get; private set; }

            public string DisplayName { get; private set; }

            public string GravatarUrl { get; private set; }

            public decimal TotalAccountValue { get; private set; }

            public decimal Profit { get; private set; }

            public decimal ProfitPercent { get; private set; }

            public LeaderBoardUser CreateLeaderBoardUser(int rank, bool isCurrentUser)
            {
                return new LeaderBoardUser(rank, isCurrentUser, DisplayName, GravatarUrl, TotalAccountValue, Profit, ProfitPercent);
            }
        }
    }
}
