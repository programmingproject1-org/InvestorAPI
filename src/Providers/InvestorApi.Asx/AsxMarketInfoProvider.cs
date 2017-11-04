using InvestorApi.Asx.Utilities;
using InvestorApi.Contracts;
using InvestorApi.Contracts.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Asx
{
    /// <summary>
    /// Provides information about the ASX Market.
    /// </summary>
    internal class AsxMarketInfoProvider : IMarketInfoProvider
    {
        private static readonly ConcurrentDictionary<int, SortedSet<DateTimeOffset>> publicHolidays =
            new ConcurrentDictionary<int, SortedSet<DateTimeOffset>>();

        private static TimeZoneInfo _timeZone = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsxMarketInfoProvider"/> class.
        /// </summary>
        public AsxMarketInfoProvider()
        {
            if (_timeZone == null)
            {
                _timeZone = TimeZoneInfo.GetSystemTimeZones()
                    .Where(t => t.Id == "Australia/Sydney" || t.Id == "AUS Eastern Standard Time")
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the market information.
        /// </summary>
        /// <returns>The market information.</returns>
        public MarketInfo GetMarket()
        {
            var currentTime = GetCurrentTime();
            var isOpen = IsMarketOpen();

            var openingTime = GetOpeningTime(currentTime);
            if (openingTime < currentTime)
            {
                openingTime = GetOpeningTime(openingTime.AddDays(1));
            }

            var closingTime = GetClosingTime(currentTime);
            if (closingTime < currentTime)
            {
                closingTime = GetClosingTime(closingTime.AddDays(1));
            }

            return new MarketInfo(currentTime, isOpen, openingTime.Subtract(currentTime), closingTime.Subtract(currentTime));
        }

        /// <summary>
        /// Gets the number of decimals to round to.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <returns>The number of decimals for the price.</returns>
        public int GetNumberOfDecimals(decimal price)
        {
            if (price <= 2.00m)
            {
                return 3;
            }

            return 2;
        }

        /// <summary>
        /// Gets the minimum step size for bid and ask prices.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <returns>The minimum step size.</returns>
        public decimal GetMinimumStepSize(decimal price)
        {
            if (price <= 0.10m)
            {
                return 0.001m;
            }

            if (price <= 2.00m)
            {
                return 0.005m;
            }

            return 0.01m;
        }

        private DateTimeOffset GetCurrentTime()
        {
            return TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _timeZone);
        }

        private bool IsMarketOpen()
        {
            return IsMarketOpen(GetCurrentTime());
        }

        private bool IsMarketOpen(DateTimeOffset time)
        {
            if (IsTradingDay(time))
            {
                var openingTime = GetOpeningTime(time);
                var closingTime = GetClosingTime(time);

                if ((time > openingTime) && (time <= closingTime))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsTradingDay()
        {
            return IsTradingDay(GetCurrentTime());
        }

        private bool IsTradingDay(DateTimeOffset date)
        {
            if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
            {
                return false;
            }

            if (GetPublicHolidays(date.Year).Contains(date.Date))
            {
                return false;
            }

            return true;
        }

        private DateTimeOffset GetOpeningTime(DateTimeOffset date)
        {
            date = TimeZoneInfo.ConvertTime(date, _timeZone);

            while (!IsTradingDay(date))
            {
                date = DateTimeOffsetUtilities.AddDays(date, _timeZone, 1);
            }

            return DateTimeOffsetUtilities.Create(_timeZone, date, 10, 00);
        }

        private DateTimeOffset GetClosingTime(DateTimeOffset date)
        {
            date = TimeZoneInfo.ConvertTime(date, _timeZone);

            while (!IsTradingDay(date))
            {
                date = DateTimeOffsetUtilities.AddDays(date, _timeZone, -1);
            }

            return DateTimeOffsetUtilities.Create(_timeZone, date, 16, 00);
        }

        private SortedSet<DateTimeOffset> GetPublicHolidays(int year)
        {
            return publicHolidays.GetOrAdd(year, y =>
            {
                var newYearsDay = PublicHolidaysUtilities.SkipWeekend(_timeZone, year, 1, 1);
                var australiaDay = PublicHolidaysUtilities.SkipWeekend(_timeZone, year, 1, 26);
                var goodFriday = PublicHolidaysUtilities.EasterSunday(_timeZone, year).AddDays(-2);
                var easterSunday = PublicHolidaysUtilities.EasterSunday(_timeZone, year);
                var easterMonday = PublicHolidaysUtilities.EasterSunday(_timeZone, year).AddDays(1);
                var anzacDay = PublicHolidaysUtilities.SkipWeekend(_timeZone, year, 4, 25);
                var queensBirthday = PublicHolidaysUtilities.NearestMonday(_timeZone, year, 6, 9);
                var labourDay = PublicHolidaysUtilities.NextDayOfWeekOccurence(_timeZone, year, 10, 1, DayOfWeek.Monday);
                var christmasDay = PublicHolidaysUtilities.SkipWeekend(_timeZone, year, 12, 25);
                var boxingDay = PublicHolidaysUtilities.SkipWeekend(_timeZone, year, 12, 26);

                if (christmasDay == boxingDay)
                {
                    boxingDay = boxingDay.AddDays(1);
                }

                return new SortedSet<DateTimeOffset>
                {
                    newYearsDay.Date,
                    australiaDay.Date,
                    goodFriday.Date,
                    easterSunday.Date,
                    easterMonday.Date,
                    anzacDay.Date,
                    queensBirthday.Date,
                    labourDay.Date,
                    christmasDay.Date,
                    boxingDay.Date
                };
            });
        }
    }
}
