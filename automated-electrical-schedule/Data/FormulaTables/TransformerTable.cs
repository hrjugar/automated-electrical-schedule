namespace automated_electrical_schedule.Data.FormulaTables;

public static class TransformerTable
{
    public const double SubBoardTransformerPrimaryProtectionFactor = 2.5;
    public const double MainBoardTransformerPrimaryProtectionFactor = 3.0;
    public const double MainBoardTransformerSecondaryProtectionFactor = 1.25;
    public const double SubBoardTransformerSecondaryProtectionGreaterEqual9 = 1.25;
    public const double SubBoardTransformerSecondaryProtectionLessThan9 = 1.67;

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
}