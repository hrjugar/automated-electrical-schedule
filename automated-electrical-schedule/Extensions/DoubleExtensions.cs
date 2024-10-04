namespace automated_electrical_schedule.Extensions;

public static class DoubleExtensions
{
    public static bool IsRoughlyEqualTo(this double value, double other, double tolerance = 0.0001)
    {
        return Math.Abs(value - other) < tolerance;
    }

    public static string ToPercentageString(this double value)
    {
        return $"{value * 100}%";
    }
}