namespace InvestorApi.Contracts.Dtos
{
    /// <summary>
    /// Contains summary information about a share.
    /// </summary>
    public class ShareInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShareInfo"/> class.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="name">The display name of the share.</param>
        /// <param name="industry">The industry.</param>
        public ShareInfo(string symbol, string name, string industry)
        {
            Symbol = symbol;
            Name = name;
            Industry = industry;
        }

        /// <summary>
        /// Gets the share symbol.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// Gets the display name of the share.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the industry.
        /// </summary>
        public string Industry { get; set; }
    }
}
