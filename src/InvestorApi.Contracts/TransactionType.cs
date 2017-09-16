namespace InvestorApi.Contracts
{
    /// <summary>
    /// Specifies the type of a transaction.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// The transaction is a money transfer.
        /// </summary>
        Transfer = 0,

        /// <summary>
        /// The transaction is a commission.
        /// </summary>
        Commission = 1,

        /// <summary>
        /// The transaction is a filled buy order.
        /// </summary>
        Buy = 2,

        /// <summary>
        /// The transaction is a filled sell order.
        /// </summary>
        Sell = 3
    }
}
