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

        switch (diff.Days)
        {
            case > 30:
                var diffMonths = diff.Days / 30;
                return $"{diffMonths} month{(diffMonths == 1 ? "" : "s")} ago";
            case > 0:
                return $"{diff.Days} day{(diff.Days == 1 ? "" : "s")} ago";
        }

        if (diff.Hours > 0) return $"{diff.Hours} hour{(diff.Hours == 1 ? "" : "s")} ago";

        return diff.Minutes > 0
            ? $"{diff.Minutes} minute{(diff.Minutes == 1 ? "" : "s")} ago"
            : $"{diff.Seconds} second{(diff.Seconds == 1 ? "" : "s")} ago";
    }
}