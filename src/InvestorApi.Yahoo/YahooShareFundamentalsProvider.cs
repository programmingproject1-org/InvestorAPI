using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace InvestorApi.Yahoo
{
    /// <summary>
    /// Implements a quote provider using Yahoo.
    /// API Documentation:
    /// http://wern-ancheta.com/blog/2015/04/05/getting-started-with-the-yahoo-finance-api/
    /// </summary>
    internal class YahooShareFundamentalsProvider : IShareFundamentalsProvider
    {
        private static readonly HttpClient _client = new HttpClient();

        private readonly IShareSummaryProvider _shareSummaryProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooShareFundamentalsProvider"/> class.
        /// </summary>
        /// <param name="shareSummaryProvider">The share summary provider.</param>
        public YahooShareFundamentalsProvider(IShareSummaryProvider shareSummaryProvider)
        {
            _shareSummaryProvider = shareSummaryProvider;
        }

        /// <summary>
        /// Gets the fundamental data for the share with the supplied symbol.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <returns>The share's fundamental data.</returns>
        public ShareFundamentals GetShareFundamentals(string symbol)
        {
            // Download the data as CSV.
            var address = $"http://download.finance.yahoo.com/d/quotes.csv?s={symbol}.AX&f=sva2pjkj1dyqr1rb4p6em4m3";
            var csv = _client.GetStringAsync(address).Result;

            // Parse the data.
            return csv
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => ReadCsvLine(line))
                .Where(quote => quote != null)
                .FirstOrDefault();
        }

        private ShareFundamentals ReadCsvLine(string line)
        {
            try
            {
                string[] values = line.Split(',');
                string symbol = values[0].Substring(1, values[0].Length - 2).Split('.').First();

                ShareSummary summary = _shareSummaryProvider.GetShareSummary(symbol);
                if (summary == null)
                {
                    return null;
                }

                return new ShareFundamentals(summary.Symbol, summary.Name, summary.Industry)
                {
                    Volume = ParseLong(values[1]),
                    AverageDailyVolume = ParseLong(values[2]),
                    PreviousClose = ParseDecimal(values[3]),
                    Low52Weeks = ParseDecimal(values[4]),
                    High52Weeks = ParseDecimal(values[5]),
                    MarketCap = ParseMarketCap(values[6]),
                    DividendShare = ParseDecimal(values[7]),
                    DividendYield = ParseDecimal(values[8]),
                    ExDividendDate = ParseDate(values[9]),
                    DividendPayDate = ParseDate(values[10]),
                    PERatio = ParseDecimal(values[11]),
                    BookValue = ParseDecimal(values[12]),
                    PriceBook = ParseDecimal(values[13]),
                    EarningsShare = ParseDecimal(values[14]),
                    MovingAverage200Days = ParseDecimal(values[15]),
                    MovingAverage50Days = ParseDecimal(values[16])
                };
            }
            catch (FormatException)
            {
                return null;
            }
        }

        private static DateTime? ParseDate(string text)
        {
            if (text == "N/A")
            {
                return null;
            }

            return DateTime.ParseExact(text.Replace("\"", ""), "M/d/yyyy", CultureInfo.InvariantCulture);
        }

        private static decimal? ParseDecimal(string text)
        {
            if (text == "N/A")
            {
                return null;
            }

            if (decimal.TryParse(text, out decimal value))
            {
                return value > 0 ? value : (decimal?)null;
            }

            return null;
        }

        private static long? ParseLong(string text)
        {
            if (text == "N/A")
            {
                return null;
            }

            if (long.TryParse(text, out long value))
            {
                return value > 0 ? value : (long?)null;
            }

            return null;
        }

        private static long? ParseMarketCap(string text)
        {
            if (text == "N/A")
            {
                return null;
            }

            char multiplierChar = text[text.Length - 1];

            if (decimal.TryParse(text.Substring(0, text.Length - 1), out decimal value))
            {
                switch (multiplierChar)
                {
                    case 'T':
                        return (long)(value * 1000000000000);
                    case 'B':
                        return (long)(value * 1000000000);
                    case 'M':
                        return (long)(value * 1000000);
                    case 'K':
                        return (long)(value * 1000);
                    default:
                        return (long)Math.Round(value);
                }
            }

            return null;
        }
    }
}
