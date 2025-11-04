namespace Birahe.EndPoint.Helpers;

public static class DateTimeHelper
{
    private static readonly TimeZoneInfo TehranTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");

    public static DateTime ToUtc(DateTime localTime)
    {
        if (localTime.Kind == DateTimeKind.Utc)
            return localTime;
        return TimeZoneInfo.ConvertTimeToUtc(localTime, TehranTimeZone);
    }

    public static DateTime ToTehran(DateTime utcTime)
    {
        if (utcTime.Kind != DateTimeKind.Utc)
            utcTime = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, TehranTimeZone);
    }
}
