using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.FormulaTables;

public static class ConductorSizeTable
{
    public static readonly List<int> Cu60Column =
    [
        15,
        20,
        30,
        40,

        55,
        70,
        85,
        100,

        115,
        130,
        155,
        185,

        210,
        240,
        260,
        275,
        315,

        370,
        395,
        400,
        445
    ];

    public static readonly List<int> Cu75Column =
    [
        20,
        25,
        35,
        50,

        65,
        85,
        100,
        115,

        140,
        155,
        190,
        220,

        255,
        285,
        305,
        325,
        375,

        435,
        470,
        480,
        530
    ];

    public static readonly List<int> Cu90Column =
    [
        25,
        30,
        40,
        55,

        75,
        95,
        115,
        130,

        150,
        170,
        205,
        240,

        285,
        320,
        345,
        360,
        425,

        490,
        530,
        535,
        595
    ];

    public static readonly List<int> Al60Column =
    [
        15,
        15,
        25,
        30,

        40,
        55,
        65,
        75,

        90,
        100,
        120,
        140,

        165,
        190,
        205,
        220,
        255,

        300,
        315,
        320,
        365
    ];

    public static readonly List<int> Al75Column =
    [
        20,
        20,
        30,
        40,

        50,
        65,
        80,
        90,

        110,
        120,
        145,
        170,

        200,
        230,
        245,
        265,
        305,

        355,
        380,
        385,
        435
    ];

    public static readonly List<int> Al90Column =
    [
        25,
        25,
        35,
        45,

        65,
        80,
        90,
        105,

        125,
        135,
        165,
        190,

        225,
        255,
        275,
        300,
        345,

        405,
        430,
        440,
        485
    ];

    private static List<int> GetColumn(ConductorType conductorType)
    {
        return conductorType.Material switch
        {
            ConductorMaterial.Copper => conductorType.TemperatureRating switch
            {
                ConductorTemperatureRating.C60 => Cu60Column,
                ConductorTemperatureRating.C75 => Cu75Column,
                ConductorTemperatureRating.C90 => Cu90Column,
                _ => throw new ArgumentOutOfRangeException(nameof(conductorType))
            },
            ConductorMaterial.Aluminum => conductorType.TemperatureRating switch
            {
                ConductorTemperatureRating.C60 => Al60Column,
                ConductorTemperatureRating.C75 => Al75Column,
                ConductorTemperatureRating.C90 => Al90Column,
                _ => throw new ArgumentOutOfRangeException(nameof(conductorType))
            },
            _ => throw new ArgumentOutOfRangeException(nameof(conductorType))
        };
    }

    public static double GetConductorSize(ConductorType conductorType, int ampereTrip, double minimumConductorSize = 0)
    {
        if (ampereTrip == 0) return 0;

        var column = GetColumn(conductorType);
        var index = column.FindIndex(columnAmpereTrip => columnAmpereTrip >= ampereTrip);

        if (index == -1) throw new ArgumentOutOfRangeException(nameof(ampereTrip));

        for (var i = index; i < DataConstants.ConductorSizes.Count; i++)
            if (DataConstants.ConductorSizes[i].IsRoughlyEqualTo(minimumConductorSize) ||
                DataConstants.ConductorSizes[i] >= minimumConductorSize)
                return DataConstants.ConductorSizes[i];

        throw new ArgumentOutOfRangeException(nameof(ampereTrip));
    }
}