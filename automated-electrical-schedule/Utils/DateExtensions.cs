namespace automated_electrical_schedule.Utils;

public static class DateExtensions
{
    private const string DatabaseDateTimeStringFormat = "yyyy-MM-dd-HH-mm-ss";

    public static string ToDatabaseString(this DateTime dateTime)
    {
        return dateTime.ToString(DatabaseDateTimeStringFormat);
    }

    public static string ToRelativeDateString(this string databaseDateTimeString)
    {
        var dateTime = DateTime.ParseExact(databaseDateTimeString, DatabaseDateTimeStringFormat, null);
        var diff = DateTime.Now - dateTime;
        var diffSeconds = diff.Seconds;

        const int minuteSeconds = 60;
        const int hourSeconds = minuteSeconds * 60;
        const int daySeconds = hourSeconds * 24;
        const int monthSeconds = daySeconds * 30;

        switch (diffSeconds)
        {
            case < minuteSeconds:
                return $"{diffSeconds} second{(diffSeconds == 1 ? "" : "s")} ago";
            case < hourSeconds:
                return $"{diff.Minutes} minute{(diff.Minutes == 1 ? "" : "s")} ago";
            case < daySeconds:
                return $"{diff.Hours} hour{(diff.Hours == 1 ? "" : "s")} ago";
            case < monthSeconds:
                return $"{diff.Days} day{(diff.Days == 1 ? "" : "s")} ago";
            default:
                var diffDays = diff.Days / 30;
                return $"{diffDays} month{(diffDays == 1 ? "" : "s")} ago";
        }
    }
}