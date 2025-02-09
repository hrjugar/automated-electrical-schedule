using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;
using automated_electrical_schedule.Data.Models;
using automated_electrical_schedule.Data.Records;
using automated_electrical_schedule.Data.Wrappers;
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

    public static CalculationResult<double> GetMotorOutlet230VoltAmpereLoad(string horsepower)
    {
        var index = DataConstants.SinglePhaseHorsepowerValues.FindIndex(hp => hp == horsepower);
        
        return index == -1
            ? CalculationResult<double>.Failure(CalculationErrorType.NoFittingHorsepower)
            : CalculationResult<double>.Success(DataConstants.SinglePhaseMotorOutletAmpereLoadRatings[index]);
    }

    public static double CalculateStandardDeviation(List<double> values)
    {
        var avg = values.Average();
        var sumOfSquares = values.Sum(value => Math.Pow(value - avg, 2));
        return Math.Sqrt(sumOfSquares / values.Count);
    }

    public static CalculationResult<double?> GetVoltageDropCorrectionConductorSize(
        LineToLineVoltage lineToLineVoltage,
        RacewayType racewayType,
        ConductorType conductorType,
        CalculationResult<double> conductorSize,
        CalculationResult<double> ampereLoad,
        double? wireLength,
        int setCount,
        int voltage
    )
    {
        if (conductorSize.HasError) return CalculationResult<double?>.Success(null);
        
        var r = VoltageDropTable.GetR(racewayType, conductorType.Material, conductorSize);
        var x = VoltageDropTable.GetX(racewayType, conductorSize);
        var voltageDrop = VoltageDropTable.GetVoltageDrop(
            lineToLineVoltage,
            r,
            x,
            ampereLoad,
            wireLength,
            setCount,
            voltage
        );
        
        if (voltageDrop.HasError) return CalculationResult<double?>.Failure(voltageDrop.ErrorType);
        if (voltageDrop.Value is null) return CalculationResult<double?>.Success(null);
        
        var conductorSizeColumnResult = ConductorSizeTable.GetColumn(conductorType);
        if (conductorSizeColumnResult.HasError) return CalculationResult<double?>.Failure(conductorSizeColumnResult.ErrorType);
        var conductorSizeColumn = conductorSizeColumnResult.Value;
        
        var index = DataConstants.ConductorSizes.FindIndex(size => size.IsRoughlyEqualTo(conductorSize.Value));
        if (index == -1) return CalculationResult<double?>.Failure(CalculationErrorType.NoFittingConductorSize);
        
        while (true)
        {
            index += 1;
            if (index == conductorSizeColumn.Count) return CalculationResult<double?>.Failure(CalculationErrorType.NoFittingConductorSizeForVoltageDropCorrection);
            var newConductorSize = CalculationResult<double>.Success(conductorSizeColumn[index]);
            
            var newR = VoltageDropTable.GetR(racewayType, conductorType.Material, newConductorSize);
            var newX = VoltageDropTable.GetX(racewayType, newConductorSize);
            var newVoltageDrop = VoltageDropTable.GetVoltageDrop(
                lineToLineVoltage,
                newR,
                newX,
                ampereLoad,
                wireLength,
                setCount,
                voltage
            );
            
            if (newVoltageDrop.HasError) return CalculationResult<double?>.Failure(newVoltageDrop.ErrorType);
            if (newVoltageDrop.Value is null) return CalculationResult<double?>.Success(null);
            if (newVoltageDrop.Value < 0.03) return CalculationResult<double?>.Success(newConductorSize.Value);
        }
        // while (true)
        // {
        //     var r = VoltageDropTable.GetR(racewayType, conductorType.Material, newConductorSize);
        //     var x = VoltageDropTable.GetX(racewayType, newConductorSize);
        //     var voltageDrop = VoltageDropTable.GetVoltageDrop(
        //         lineToLineVoltage,
        //         r,
        //         x,
        //         ampereLoad,
        //         wireLength,
        //         setCount,
        //         voltage
        //     );
        //     
        //     if (voltageDrop.HasError) return CalculationResult<double?>.Failure(voltageDrop.ErrorType);
        //     if (voltageDrop.Value is null) return CalculationResult<double?>.Success(null);
        //     // if (voltageDrop.Value is null || voltageDrop.Value < 0.03) return CalculationResult<double?>.Success(null);
        //
        //     var conductorSizeColumnResult = ConductorSizeTable.GetColumn(conductorType);
        //     if (conductorSizeColumnResult.HasError) return CalculationResult<double?>.Failure(conductorSizeColumnResult.ErrorType);
        //     var conductorSizeColumn = conductorSizeColumnResult.Value;
        //     
        //     var index = DataConstants.ConductorSizes.FindIndex(size => size.IsRoughlyEqualTo(newConductorSize.Value));
        //
        //     if (index == -1 || index == conductorSizeColumn.Count - 1)
        //     {
        //         return CalculationResult<double?>.Failure(CalculationErrorType.NoFittingConductorSizeForVoltageDropCorrection);
        //     }
        //     
        //     newConductorSize = CalculationResult<double>.Success(conductorSizeColumn[index + 1]);
        // }
    }
}