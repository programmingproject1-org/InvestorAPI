using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace InvestorApi.Yahoo
{
    /// <summary>
    /// Provides share dividend history data using Yahoo.
    /// </summary>
    /// <remarks>
    /// Code for fetching cookie and crumb is Copyright by Dennis Lee, taken from:
    /// https://stackoverflow.com/questions/44030983/yahoo-finance-url-not-working
    /// </remarks>
    internal sealed class YahooShareDividendHistoryProvider : IShareDividendHistoryProvider
    {
        private static readonly Regex regex = new Regex("CrumbStore\":{\"crumb\":\"(?<crumb>.+?)\"}", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// Gets the dividend history for a share.
        /// </summary>
        /// <param name="symbol">The share symbol.</param>
        /// <param name="range">The date range. Possible values are: 1y, 2y, 5y, 10y, max</param>
        /// <returns>The dividend history.</returns>
        public IReadOnlyCollection<Dividend> GetDividendHistory(string symbol, string range)
        {
            // Get the access cookie and crumb.
            Token token = Authenticate(symbol);
            if (token == null)
            {
                return null;
            }

            // Parse the range.
            if (!int.TryParse(range.Substring(0, range.Length - 1), out int years))
            {
                years = 25;
            }

            // Download the CSV.
            var csv = DownloadDividendHistory(token, DateTimeOffset.UtcNow.AddYears(-years), DateTimeOffset.UtcNow);

            // Parse the data.
            return csv
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(line => ReadCsvLine(line))
                .OrderBy(div => div.Date)
                .Where(quote => quote != null)
                .ToList();
        }

        private static Dividend ReadCsvLine(string line)
        {
            try
            {
                var values = line.Split(',');

                var date = DateTime.ParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var value = decimal.Parse(values[1]);

                return new Dividend(date, value);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        private static Token Authenticate(string symbol)
        {
            // Prepare the request.
            string url = string.Format("https://finance.yahoo.com/quote/{0}.AX/history", symbol);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    // Read the HTML content from the response and try to find the crumb value in it.
                    MatchCollection matches = regex.Matches(reader.ReadToEnd());
                    if (matches.Count > 0)
                    {
                        return new Token
                        {
                            Symbol = symbol,
                            Cookie = response.GetResponseHeader("Set-Cookie").Split(';')[0],
                            Crumb = matches[0].Groups["crumb"].Value.Replace("\\u002F", "/")
                        };
                    }

                    return null;
                }
            }
        }

        private static string DownloadDividendHistory(Token token, DateTimeOffset start, DateTimeOffset end)
        {
            // Prepare the request including the web site cookie.
            var url = string.Format("https://query1.finance.yahoo.com/v7/finance/download/{0}.AX?period1={1}&period2={2}&interval=1d&events=div&crumb={3}",
                token.Symbol,
                start.ToUnixTimeSeconds(),
                end.ToUnixTimeSeconds(),
                token.Crumb);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add("Cookie", token.Cookie);
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        // Read the CSV content from the response.
                        return new StreamReader(stream).ReadToEnd();
                    }
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
            {
                // Share with supplied symbol not found.
                return null;
            }
        }

        private class Token
        {
            public string Symbol { get; set; }

            public string Cookie { get; set; }

            public string Crumb { get; set; }
        }
    }
}
