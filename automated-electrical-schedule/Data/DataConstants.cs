namespace automated_electrical_schedule.Data;

public static class DataConstants
{
    public static readonly List<int> StandardAmpereTripRatings =
    [
        15,
        20,
        25,
        30,
        35,
        40,
        45,
        50,
        60,
        70,
        80,
        90,
        100,
        110,
        125,
        150,
        175,
        200,
        225,
        250,
        300,
        350,
        400,
        450,
        500,
        600,
        700,
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

    public static readonly List<int> StandardAmpereFrameRatings =
    [
        50,
        60,
        70,
        80,
        90,
        100,
        110,
        125,
        150,
        175,
        200,
        225,
        250,
        300,
        350,
        400,
        450,
        500,
        600,
        700,
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

    public static readonly List<double> MotorOutlet230VoltAmpereLoadRatings =
    [
        2.2,
        2.9,
        3.6,
        4.9,
        6.9,
        8,
        10,
        12,
        17,
        28,
        40,
        50
    ];

    public static readonly List<double> ConductorSizes =
    [
        2,
        3.5,
        5.5,
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
        150,
        175,
        200,
        250,

        325,
        375,
        400,
        500
    ];
}