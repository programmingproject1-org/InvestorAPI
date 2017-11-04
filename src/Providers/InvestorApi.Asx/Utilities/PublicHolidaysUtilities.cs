using System;

namespace InvestorApi.Asx.Utilities
{
    internal static class PublicHolidaysUtilities
    {
        /// <remarks>
        /// Source: https://stackoverflow.com/questions/2510383/how-can-i-calculate-what-date-good-friday-falls-on-given-a-year.
        /// </remarks>
        public static DateTimeOffset EasterSunday(TimeZoneInfo timeZone, int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return DateTimeOffsetUtilities.Create(timeZone, year, month, day);
        }

        public static DateTimeOffset NextDayOfWeekOccurence(DateTimeOffset date, DayOfWeek dayOfWeek)
        {
            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1);
            }

            return date;
        }

        public static DateTimeOffset NextDayOfWeekOccurence(TimeZoneInfo timeZone, int year, int month, int day, DayOfWeek dayOfWeek)
        {
            return NextDayOfWeekOccurence(DateTimeOffsetUtilities.Create(timeZone, year, month, day), dayOfWeek);
        }

        public static DateTimeOffset NearestMonday(DateTimeOffset date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Tuesday:
                    return date.AddDays(-1);
                case DayOfWeek.Wednesday:
                    return date.AddDays(-2);
                case DayOfWeek.Thursday:
                    return date.AddDays(-3);
                case DayOfWeek.Friday:
                    return date.AddDays(3);
                case DayOfWeek.Saturday:
                    return date.AddDays(2);
                case DayOfWeek.Sunday:
                    return date.AddDays(1);
                default:
                    return date;
            }
        }

        public static DateTimeOffset NearestMonday(TimeZoneInfo timeZone, int year, int month, int day)
        {
            return NearestMonday(DateTimeOffsetUtilities.Create(timeZone, year, month, day));
        }

        public static DateTimeOffset SkipWeekend(DateTimeOffset date)
        {
            while ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
            {
                date = date.AddDays(1);
            }

            return date;
        }

        public static DateTimeOffset SkipWeekend(TimeZoneInfo timeZone, int year, int month, int day)
        {
            return SkipWeekend(DateTimeOffsetUtilities.Create(timeZone, year, month, day));
        }
    }
}
