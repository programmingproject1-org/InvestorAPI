namespace InvestorApi.Models
{
    /// <summary>
    /// Specifies the side of an order.
    /// </summary>
    public enum OrderSide
    {
        /// <summary>
        /// The side has not been specified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Buy order
        /// </summary>
        Buy,

        /// <summary>
        /// Sell order
        /// </summary>
        Sell
    }
}
