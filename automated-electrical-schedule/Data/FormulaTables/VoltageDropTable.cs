using automated_electrical_schedule.Data.Enums;
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

    public static double GetX(RacewayType racewayType, double conductorSize)
    {
        if (conductorSize == 0) return 0;

        var column = racewayType switch
        {
            RacewayType.Pvc or
                RacewayType.Ent or
                RacewayType.Emt or
                RacewayType.Fmc => XColumnOneValues,
            RacewayType.Imc or
                RacewayType.Rmc => XColumnTwoValues,
            _ => throw new ArgumentOutOfRangeException(nameof(racewayType))
        };
        return column[DataConstants.ConductorSizes.FindIndex(size => size.IsRoughlyEqualTo(conductorSize))];
    }

    private static List<double> GetRColumn(RacewayType racewayType, ConductorMaterial conductorMaterial)
    {
        return conductorMaterial switch
        {
            ConductorMaterial.Copper => racewayType switch
            {
                RacewayType.Pvc or RacewayType.Ent => RCuColumnOneValues,
                RacewayType.Emt or RacewayType.Fmc => RCuColumnTwoValues,
                RacewayType.Imc or RacewayType.Rmc => RCuColumnThreeValues,
                _ => throw new ArgumentOutOfRangeException(nameof(racewayType))
            },
            ConductorMaterial.Aluminum => racewayType switch
            {
                RacewayType.Pvc or RacewayType.Ent => RAlColumnOneValues,
                RacewayType.Emt or RacewayType.Fmc => RAlColumnTwoValues,
                RacewayType.Imc or RacewayType.Rmc => RAlColumnThreeValues,
                _ => throw new ArgumentOutOfRangeException(nameof(racewayType))
            },
            _ => throw new ArgumentOutOfRangeException(nameof(conductorMaterial))
        };
    }

    public static double GetR(RacewayType racewayType, ConductorMaterial conductorMaterial, double conductorSize)
    {
        if (conductorSize == 0) return 0;

        var column = GetRColumn(racewayType, conductorMaterial);
        return column[DataConstants.ConductorSizes.FindIndex(size => size.IsRoughlyEqualTo(conductorSize))];
    }

    public static double GetVoltageDrop(
        LineToLineVoltage? lineToLineVoltage,
        double r,
        double x,
        double ampereLoad,
        double wireLength,
        int setCount,
        int voltage)
    {
        if (r == 0 || x == 0 || ampereLoad == 0 || wireLength == 0 || setCount == 0 || voltage == 0) return 0;

        var factor = lineToLineVoltage == LineToLineVoltage.Abc ? Math.Sqrt(3) : 2;

        return factor * ampereLoad * Math.Sqrt(Math.Pow(r, 2) + Math.Pow(x, 2)) * (wireLength / (305 * setCount)) *
               (1.0 / voltage);
    }
}