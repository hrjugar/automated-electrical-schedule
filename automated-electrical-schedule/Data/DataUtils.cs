using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data;

public static class DataUtils
{
    public static CalculationResult<int> GetAmpereTrip(CalculationResult<double> value, int minimumAmpereTrip = 0)
    {
        if (value.HasError) return CalculationResult<int>.Failure(value.ErrorType);
        
        int? result = DataConstants.StandardAmpereTripRatings
            .FirstOrDefault(columnAmpereTrip => columnAmpereTrip >= minimumAmpereTrip && columnAmpereTrip >= value.Value);
        
        return result is null
            ? CalculationResult<int>.Failure(CalculationErrorType.NoFittingAmpereTrip)
            : CalculationResult<int>.Success(result.Value);
    }

    public static CalculationResult<int> GetAmpereFrame(CalculationResult<int> ampereTrip)
    {
        if (ampereTrip.HasError) return CalculationResult<int>.Failure(ampereTrip.ErrorType);
        
        int? result = DataConstants.StandardAmpereFrameRatings.FirstOrDefault(ampereFrame => ampereFrame >= ampereTrip.Value);

        return result is null
            ? CalculationResult<int>.Failure(CalculationErrorType.NoFittingAmpereFrame)
            : CalculationResult<int>.Success(result.Value);
    }

    public static CalculationResult<double> GetMotorOutlet230VoltAmpereLoad(double horsepower)
    {
        var index = DataConstants.SinglePhaseHorsepowerValues.FindIndex(hp => hp.IsRoughlyEqualTo(horsepower));
        
        return index == -1
            ? CalculationResult<double>.Failure(CalculationErrorType.NoFittingHorsepower)
            : CalculationResult<double>.Success(DataConstants.SinglePhaseMotorOutletAmpereLoadRatings[index]);
    }
}