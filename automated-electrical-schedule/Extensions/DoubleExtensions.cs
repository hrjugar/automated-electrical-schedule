using System.Globalization;

namespace automated_electrical_schedule.Extensions;

public static class DoubleExtensions
{
    // USE ONLY FOR HORSEPOWER VALUES

    private static readonly Dictionary<double, string> DoubleHorsepowerFractions = new()
    {
        { 1.0 / 6, "1/6" },
        { 1.0 / 4, "1/4" },
        { 1.0 / 3, "1/3" },
        { 1.0 / 2, "1/2" },
        { 3.0 / 4, "3/4" },
        { 1.5, "1 1/2" },
        { 7.5, "7 1/2" }
    };

    public static bool IsRoughlyEqualTo(this double value, double other, double tolerance = 0.0001)
    {
        return Math.Abs(value - other) < tolerance;
    }

    public static string ToRoundedString(this double value, bool isPercentage = false)
    {
        var roundedValue = Math.Round(value, 4);
        if (isPercentage) return $"{Math.Round(value * 100, 2)}%";
        return roundedValue.ToString(CultureInfo.InvariantCulture);
    }
    
    public static string ToRoundedString(this double? value, bool isPercentage = false)
    {
        if (value is null) return string.Empty;
        return value.Value.ToRoundedString(isPercentage);
    }

    public static string ToPercentageString(this double value)
    {
        return $"{Math.Round(value * 100, 2)}%";
    }

    public static string ToHorsepowerFractionalString(this double horsepower)
    {
        foreach (var (key, val) in DoubleHorsepowerFractions)
            if (horsepower.IsRoughlyEqualTo(key))
                return val;

        return horsepower.ToString(CultureInfo.InvariantCulture);
    }
}