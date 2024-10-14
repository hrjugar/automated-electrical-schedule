using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;

namespace automated_electrical_schedule.Data.FormulaTables;

public static class CableTrayRacewaySizeTable
{
    private static readonly Dictionary<int, int> cableTrayRacewayDict = new()
    {
        { 1400, 50 },
        { 2800, 100 },
        { 4200, 150 },
        { 5600, 200 },
        { 6100, 225 },
        { 8400, 300 },
        { 11200, 400 },
        { 12600, 450 },
        { 14000, 500 },
        { 16800, 600 },
        { 21000, 750 },
        { 25200, 900 }
    };

    public static CalculationResult<int> GetCableTrayRacewaySize(
        int setCount,
        int conductorWireCount,
        CalculationResult<double> conductorWireSize,
        int groundingWireCount,
        CalculationResult<double> groundingWireSize)
    {
        if (conductorWireSize.HasError) return CalculationResult<int>.Failure(conductorWireSize.ErrorType);
        if (groundingWireSize.HasError) return CalculationResult<int>.Failure(groundingWireSize.ErrorType);

        var allowableFillArea =
            setCount * (conductorWireCount * conductorWireSize.Value + groundingWireCount * groundingWireSize.Value);

        if (allowableFillArea > cableTrayRacewayDict.Keys.Max()) return CalculationResult<int>.Failure(CalculationErrorType.NoFittingFillArea);
        
        int? closestMaxAllowableFillArea = cableTrayRacewayDict.Keys.FirstOrDefault(fillArea => fillArea >= allowableFillArea);
        
        return closestMaxAllowableFillArea is null
            ? CalculationResult<int>.Failure(CalculationErrorType.NoFittingRacewaySize)
            : CalculationResult<int>.Success(cableTrayRacewayDict[closestMaxAllowableFillArea.Value]);
    }
}