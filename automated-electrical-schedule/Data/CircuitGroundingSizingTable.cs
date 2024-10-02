using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data;

public static class CircuitGroundingSizingTable
{
    public static readonly List<int> AmpereTripRatings =
    [
        15,
        20,
        30,

        40,
        60,
        100,

        200,
        300,
        400,

        500,
        600,
        800,

        1000,
        1200,
        1600,

        2000,
        2500,
        3000,

        4000,
        5000,
        6000
    ];

    public static readonly List<double> CuColumn =
    [
        2,
        3.5,
        5.5,

        5.5,
        5.5,
        8,

        14,
        22,
        30,

        30,
        38,
        50,

        60,
        80,
        100,

        125,
        175,
        200,

        250,
        375,
        400
    ];

    public static readonly List<double> AlColumn =
    [
        3.5,
        5.5,
        8,

        8,
        8,
        14,

        22,
        30,
        38,

        50,
        60,
        80,

        100,
        125,
        175,

        200,
        325,
        325,

        375,
        600,
        600
    ];

    public static double GetGroundingSize(ConductorMaterial conductorMaterial, int ampereTrip)
    {
        var column = conductorMaterial == ConductorMaterial.Copper ? CuColumn : AlColumn;
        var index = AmpereTripRatings.FindIndex(at => at >= ampereTrip);

        return column[index];
    }
}