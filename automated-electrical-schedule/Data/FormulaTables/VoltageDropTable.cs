using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.FormulaTables;

public static class VoltageDropTable
{
    private static readonly List<double> XColumnOneValues =
    [
        0.058,
        0.054,
        0.05,
        0.052,
        0.051,
        0.048,
        0.045,
        0.046,
        0.044,
        0.043,
        0.042,
        0.041,
        0.041,
        0.041,
        0.04,
        0.039,
        0.039,
        0.039,
        0.038,
        0.038,
        0.037
    ];

    private static readonly List<double> XColumnTwoValues =
    [
        0.073,
        0.068,
        0.063,
        0.065,
        0.064,
        0.06,
        0.057,
        0.057,
        0.055,
        0.054,
        0.052,
        0.051,
        0.052,
        0.051,
        0.05,
        0.049,
        0.048,
        0.048,
        0.048,
        0.048,
        0.046
    ];


    private static readonly List<double> RCuColumnOneValues =
    [
        3.1,
        2.0,
        1.2,
        0.78,
        0.49,
        0.31,
        0.19,
        0.15,
        0.12,
        0.1,
        0.077,
        0.062,
        0.052,
        0.044,
        0.038,
        0.033,
        0.027,
        0.023,
        0.019,
        0.019,
        0.015
    ];

    private static readonly List<double> RCuColumnTwoValues =
    [
        3.1,
        2.0,
        1.2,
        0.78,
        0.49,
        0.31,
        0.2,
        0.16,
        0.12,
        0.1,
        0.082,
        0.067,
        0.057,
        0.049,
        0.043,
        0.038,
        0.032,
        0.028,
        0.024,
        0.024,
        0.019
    ];

    private static readonly List<double> RCuColumnThreeValues =
    [
        3.1,
        2.0,
        1.2,
        0.78,
        0.49,
        0.31,
        0.2,
        0.16,
        0.12,
        0.1,
        0.079,
        0.063,
        0.054,
        0.45,
        0.039,
        0.035,
        0.029,
        0.025,
        0.021,
        0.021,
        0.018
    ];

    private static readonly List<double> RAlColumnOneValues =
    [
        3.2,
        3.2,
        2,
        1.3,
        0.81,
        0.51,
        0.32,
        0.25,
        0.2,
        0.16,
        0.13,
        0.1,
        0.085,
        0.071,
        0.061,
        0.054,
        0.043,
        0.036,
        0.029,
        0.029,
        0.023
    ];

    private static readonly List<double> RAlColumnTwoValues =
    [
        3.2,
        3.2,
        2,
        1.3,
        0.81,
        0.51,
        0.32,
        0.26,
        0.21,
        0.16,
        0.13,
        0.11,
        0.09,
        0.076,
        0.066,
        0.059,
        0.048,
        0.041,
        0.034,
        0.034,
        0.027
    ];

    private static readonly List<double> RAlColumnThreeValues =
    [
        3.2,
        3.2,
        2,
        1.3,
        0.81,
        0.51,
        0.32,
        0.25,
        0.2,
        0.16,
        0.13,
        0.1,
        0.086,
        0.072,
        0.063,
        0.055,
        0.045,
        0.038,
        0.031,
        0.031,
        0.025
    ];

    public static CalculationResult<double> GetX(RacewayType racewayType, CalculationResult<double> conductorSize)
    {
        if (conductorSize.HasError) return CalculationResult<double>.Failure(conductorSize.ErrorType);
        
        List<double> column;
        switch (racewayType)
        {
            case RacewayType.Pvc:
            case RacewayType.Ent:
            case RacewayType.Emt:
            case RacewayType.Fmc:
                column = XColumnOneValues;
                break;
            case RacewayType.Imc:
            case RacewayType.Rmc:
                column = XColumnTwoValues;
                break;
            case RacewayType.CableTray:
            default:
                return CalculationResult<double>.Failure(CalculationErrorType.InvalidRacewayType);
        }
        
        var index = DataConstants.ConductorSizes.FindIndex(size => size.IsRoughlyEqualTo(conductorSize.Value));

        return index == -1
            ? CalculationResult<double>.Failure(CalculationErrorType.NoFittingConductorSize)
            : CalculationResult<double>.Success(column[index]);
    }

    public static CalculationResult<double> GetR(RacewayType racewayType, ConductorMaterial conductorMaterial, CalculationResult<double> conductorSize)
    {
        if (conductorSize.HasError) return CalculationResult<double>.Failure(conductorSize.ErrorType);

        List<double> column;
        switch (conductorMaterial)
        {
            case ConductorMaterial.Copper:
                switch (racewayType)
                {
                    case RacewayType.Pvc:
                    case RacewayType.Ent:
                        column = RCuColumnOneValues;
                        break;
                    case RacewayType.Emt:
                    case RacewayType.Fmc:
                        column = RCuColumnTwoValues;
                        break;
                    case RacewayType.Imc:
                    case RacewayType.Rmc:
                        column = RCuColumnThreeValues;
                        break;
                    case RacewayType.CableTray:
                    default:
                        return CalculationResult<double>.Failure(CalculationErrorType.InvalidRacewayType);
                }

                break;
            case ConductorMaterial.Aluminum:
                switch (racewayType)
                {
                    case RacewayType.Pvc:
                    case RacewayType.Ent:
                        column = RAlColumnOneValues;
                        break;
                    case RacewayType.Emt:
                    case RacewayType.Fmc:
                        column = RAlColumnTwoValues;
                        break;
                    case RacewayType.Imc:
                    case RacewayType.Rmc:
                        column = RAlColumnThreeValues;
                        break;
                    case RacewayType.CableTray:
                    default:
                        return CalculationResult<double>.Failure(CalculationErrorType.InvalidRacewayType);
                }

                break;
            default:
                return CalculationResult<double>.Failure(CalculationErrorType.InvalidConductorMaterial);
        }
        
        var index = DataConstants.ConductorSizes.FindIndex(size => size.IsRoughlyEqualTo(conductorSize.Value));
        
        return index == -1
            ? CalculationResult<double>.Failure(CalculationErrorType.NoFittingConductorSize)
            : CalculationResult<double>.Success(column[index]);
    }

    public static CalculationResult<double> GetVoltageDrop(LineToLineVoltage? lineToLineVoltage,
        CalculationResult<double> r,
        CalculationResult<double> x,
        CalculationResult<double> ampereLoad,
        double wireLength,
        int setCount,
        int voltage)
    {
        if (r.HasError) return CalculationResult<double>.Failure(r.ErrorType);
        if (x.HasError) return CalculationResult<double>.Failure(x.ErrorType);
        if (ampereLoad.HasError) return CalculationResult<double>.Failure(ampereLoad.ErrorType);

        var factor = lineToLineVoltage == LineToLineVoltage.Abc ? Math.Sqrt(3) : 2;

        var value = factor * ampereLoad.Value * Math.Sqrt(Math.Pow(r.Value, 2) + Math.Pow(x.Value, 2)) * (wireLength / (305 * setCount)) *
               (1.0 / voltage);
        
        return CalculationResult<double>.Success(value);
    }
}