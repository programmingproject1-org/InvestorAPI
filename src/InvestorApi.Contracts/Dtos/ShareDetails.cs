namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains detailed information about a share.
    /// </summary>
    public class ShareDetails : ShareInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShareDetails"/> class.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="name">The display name of the share.</param>
        /// <param name="industry">The industry.</param>
        public ShareDetails(string symbol, string name, string industry)
            : base(symbol, name)
        {
            Industry = industry;
        }

        /// <summary>
        /// Gets or sets the industry.
        /// </summary>
        public string Industry { get; set; }
    }
}
