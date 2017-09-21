namespace InvestorApi.Models
{
    internal static class ValidationRegularExpressions
    {
        public const string EmailAddress = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public const string AlphaNumeric = @"^[^~`^$#@%!'*\(\)<>=.;:]+$";

        public const string Symbol = @"^[A-Za-z0-9]*$";
    }
}
