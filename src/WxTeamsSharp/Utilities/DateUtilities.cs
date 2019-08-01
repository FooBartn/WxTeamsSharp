using System;

namespace WxTeamsSharp.Utilities
{
    internal static class DateUtilities
    {
        internal static string ToFormattedUTCTime(this DateTimeOffset dateTime)
            => dateTime.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
    }
}
