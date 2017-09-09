namespace InvestorApi.Models
{
    /// <summary>
    /// The response of a login operation.
    /// </summary>
    public sealed class LoginResponse
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds until the token expires.
        /// </summary>
        public int Expires { get; set; }
    }
}
