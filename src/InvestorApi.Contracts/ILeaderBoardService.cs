using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// Provides leader board information.
    /// </summary>
    public interface ILeaderBoardService
    {
        /// <summary>
        /// Gets the leader board users.
        /// </summary>
        /// <param name="currentUserId">The unique identifier of the current user.</param>
        /// <param name="pageNumber">Gets the page number to return.</param>
        /// <param name="pageSize">Gets the page size to apply.</param>
        /// <returns>The leader board users.</returns>
        ListResult<LeaderBoardUser> GetUsers(Guid currentUserId, int pageNumber, int pageSize);

        /// <summary>
        /// Gets the leader board users around and including the supplied user.
        /// </summary>
        /// <param name="currentUserId">The unique identifier of the user to get the leader board for.</param>
        /// <param name="neighborCount">The number of neighbors to include</param>
        /// <returns>The leader board users.</returns>
        IReadOnlyCollection<LeaderBoardUser> GetUsers(Guid currentUserId, int neighborCount);
    }
}
