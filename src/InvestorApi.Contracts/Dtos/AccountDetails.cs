using System;
using System.Collections.Generic;

namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains detailed information about a trading account.
    /// </summary>
    public class AccountDetails : AccountInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountDetails"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the account.</param>
        /// <param name="name">The account name.</param>
        /// <param name="balance">The current account balance.</param>
        /// <param name="positions">The current positions in the account.</param>
        public AccountDetails(Guid id, string name, decimal balance, IReadOnlyCollection<PositionInfo> positions)
            : base(id, name, balance)
        {
            Positions = positions;
        }

        /// <summary>
        /// Gets the the current positions in the account.
        /// </summary>
        public IReadOnlyCollection<PositionInfo> Positions { get; }
    }
}
