using InvestorApi.Contracts.Dtos;

namespace InvestorApi.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IShareFundamentalsProvider
    {
        /// <summary>
        /// Gets the fundamental data for the share with the supplied symbol.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <returns>The share's fundamental data.</returns>
        ShareFundamentals GetShareFundamentals(string symbol);
    }
}
