using System;

namespace InvestorApi.Asx.Utilities
{
    internal static class DateTimeOffsetUtilities
    {
        public static DateTimeOffset AddDays(DateTimeOffset currentTime, TimeZoneInfo timeZone, int count)
        {
            var utc = TimeZoneInfo.ConvertTime(currentTime, TimeZoneInfo.Utc).AddDays(count);
            return TimeZoneInfo.ConvertTime(utc, timeZone);
        }

        public static DateTimeOffset Create(TimeZoneInfo timeZone, DateTimeOffset date, int hour, int minute)
        {
            date = TimeZoneInfo.ConvertTime(date, timeZone);
            return Create(timeZone, date.Year, date.Month, date.Day, hour, minute, 0);
        }

        public static DateTimeOffset Create(TimeZoneInfo timeZone, int year, int month, int day)
        {
            return Create(timeZone, year, month, day, 0, 0, 0);
        }

        public static DateTimeOffset Create(TimeZoneInfo timeZone, int year, int month, int day, int hour, int minute, int second)
        {
            var unspecifiedTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified);
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(unspecifiedTime, timeZone);
            return TimeZoneInfo.ConvertTime(new DateTimeOffset(utcTime), timeZone);
        }
    }
}
