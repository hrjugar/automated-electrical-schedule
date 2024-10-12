namespace automated_electrical_schedule.Data.FormulaTables;

public static class TransformerTable
{
    public const double TransformerPrimaryProtectionFactor = 2.5;

    public static readonly List<int> TransformerRatings =
    [
        3000,
        6000,
        9000,
        15000,
        30000,
        45000,
        75000,
        112500,
        150000,
        225000,
        300000,
        500000,
        750000,
        1000000
    ];

    public static int GetTransformerRating(double value)
    {
        if (value == 0) return 0;

        return TransformerRatings.First(rating => rating >= value);
    }

    public static double GetTransformerSecondaryProtectionFactor(double value)
    {
        return value >= 9 ? 1.25 : 1.67;
    }
}