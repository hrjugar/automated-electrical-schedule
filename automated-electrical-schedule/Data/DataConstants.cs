using automated_electrical_schedule.Data.Models;

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

    public static int GetAmpereTrip(double value, int minimumAmpereTrip = 0)
    {
        return StandardAmpereTripRatings
            .First(columnAmpereTrip => columnAmpereTrip >= minimumAmpereTrip && columnAmpereTrip >= value);
    }

    public static int GetAmpereFrame(int ampereTrip)
    {
        return StandardAmpereFrameRatings.First(ampereFrame => ampereFrame >= ampereTrip);
    }

    public static double GetMotorOutlet230VoltAmpereLoad(double horsepower)
    {
        return MotorOutlet230VoltAmpereLoadRatings[
            MotorOutletCircuit.AllowedHorsepowerValues.FindIndex(hp => Math.Abs(hp - horsepower) < 0.001)];
    }
}