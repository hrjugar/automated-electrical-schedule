using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;

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

    public static CalculationResult<int> GetTransformerRating(CalculationResult<double> value)
    {
        if (value.HasError) return CalculationResult<int>.Failure(value.ErrorType);
        if (value.Value > TransformerRatings.Max()) return CalculationResult<int>.Failure(CalculationErrorType.NoFittingTransformerRating);
        return CalculationResult<int>.Success(TransformerRatings.First(rating => rating >= value.Value));
    }
}