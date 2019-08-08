namespace WxTeamsSharp.Extensions
{
    internal static class Date
    {
        internal static string ToFormattedUTCTime(this System.DateTimeOffset dateTime)
            => dateTime.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
    }
}
