using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace InvestorApi.Domain.Utilities
{
    internal class AsxMarket
    {
        private static readonly ConcurrentDictionary<int, SortedSet<DateTimeOffset>> publicHolidays =
            new ConcurrentDictionary<int, SortedSet<DateTimeOffset>>();

        private static TimeZoneInfo _timeZone = null;

        public AsxMarket()
        {
            if (_timeZone == null)
            {
                _timeZone = TimeZoneInfo.GetSystemTimeZones()
                    .Where(t => t.Id == "Australia/Sydney" || t.Id == "AUS Eastern Standard Time")
                    .FirstOrDefault();
            }
        }

        public DateTimeOffset GetCurrentTime()
        {
            return TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _timeZone);
        }

        public bool IsMarketOpen()
        {
            return IsMarketOpen(GetCurrentTime());
        }

        public bool IsMarketOpen(DateTimeOffset time)
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

        public bool IsTradingDay()
        {
            return IsTradingDay(GetCurrentTime());
        }

        public bool IsTradingDay(DateTimeOffset date)
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

        public DateTimeOffset GetOpeningTime(DateTimeOffset date)
        {
            date = TimeZoneInfo.ConvertTime(date, _timeZone);

            while (!IsTradingDay(date))
            {
                date = DateTimeOffsetUtilities.AddDays(date, _timeZone, 1);
            }

            return DateTimeOffsetUtilities.Create(_timeZone, date, 10, 00);
        }

        public DateTimeOffset GetClosingTime(DateTimeOffset date)
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
