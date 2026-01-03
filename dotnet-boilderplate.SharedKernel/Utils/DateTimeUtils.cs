namespace dotnet_boilderplate.SharedKernel.Utils
{
    /// <summary>
    /// DateTimeUtils helps convert between Unix timestamp and DateTime in .NET.
    ///
    /// Main goals:
    /// - Support apps that work with many time zones. We always use UTC inside the app.
    /// - Frontend usually sends/receives Unix timestamp (seconds or milliseconds) – this is a common standard and has no time zone problems.
    /// - Backend stores time as timestamptz in PostgreSQL (saved as UTC inside the database).
    /// - Make sure all DateTime values in the code have Kind = Utc so EF Core and Npgsql save/read correctly.
    ///
    /// How it works:
    /// - Unix timestamp is the number of seconds (or milliseconds) since 1970-01-01 00:00:00 UTC (called Unix Epoch).
    /// - All methods here return or expect DateTime with Kind = Utc to avoid time zone mistakes.
    /// - When saving to PostgreSQL (timestamptz), Npgsql automatically handles the conversion to UTC.
    /// - When reading from the database, Npgsql returns DateTime with Kind = Utc – the exact moment in time, no matter what time zone the user is in.
    ///
    /// Short info about Unix time:
    /// - Unix time (also called Epoch time) counts seconds from January 1, 1970, 00:00:00 UTC.
    /// - It does not store time zone information, so it is perfect for sending data between systems in different time zones.
    /// - More info: https://en.wikipedia.org/wiki/Unix_time
    ///
    /// Note:
    /// - Methods in #region RemoveLater are only for testing/reference and will be removed later when the main logic is confirmed stable.
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// Convert Unix timestamp in seconds to DateTime in UTC.
        /// </summary>
        public static DateTime FromUnixSeconds(long unixSeconds)
        {
            return DateTime.UnixEpoch.AddSeconds(unixSeconds).ToUniversalTime();
        }

        /// <summary>
        /// Convert Unix timestamp in milliseconds to DateTime in UTC.
        /// </summary>
        public static DateTime FromUnixMilliseconds(long unixMilliseconds)
        {
            return DateTime.UnixEpoch.AddMilliseconds(unixMilliseconds).ToUniversalTime();
        }

        /// <summary>
        /// Convert a UTC DateTime back to Unix seconds.
        /// </summary>
        public static long ToUnixSeconds(DateTime utcDateTime)
        {
            if (utcDateTime.Kind == DateTimeKind.Utc)
                utcDateTime = utcDateTime.ToUniversalTime();

            return (long)(utcDateTime - DateTime.UnixEpoch).TotalSeconds;
        }

        /// <summary>
        /// Convert a UTC DateTime back to Unix milliseconds.
        /// </summary>
        public static long ToUnixMilliseconds(DateTime utcDateTime)
        {
            if (utcDateTime.Kind != DateTimeKind.Utc)
                utcDateTime = utcDateTime.ToUniversalTime();

            return (long)(utcDateTime - DateTime.UnixEpoch).TotalMilliseconds;
        }

        /// <summary>
        /// Convert a UTC DateTime to a specific time zone (for display to users).
        /// Use IANA time zone names like "Asia/Ho_Chi_Minh" or "America/New_York".
        /// </summary>
        public static DateTime ToTimeZone(DateTime utcDateTime, string ianaTimeZoneId)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(ianaTimeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
        }

        #region RemoveLater
        // These methods are kept only for quick testing.
        // They will be removed once the main methods are stable.
        public static DateTime FromUnixSecondsNoUniversal(long unixSeconds)
        {
            return DateTime.UnixEpoch.AddSeconds(unixSeconds);
        }

        public static DateTime FromUnixSecondsToLocalTime(long unixSeconds)
        {
            return DateTime.UnixEpoch.AddSeconds(unixSeconds).ToLocalTime();
        }
        #endregion
    }
}
