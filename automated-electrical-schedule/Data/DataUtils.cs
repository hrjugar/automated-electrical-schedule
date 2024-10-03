using automated_electrical_schedule.Data.Models;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data;

public static class DataUtils
{
    public static int GetAmpereTrip(double value, int minimumAmpereTrip = 0)
    {
        return DataConstants.StandardAmpereTripRatings
            .First(columnAmpereTrip => columnAmpereTrip >= minimumAmpereTrip && columnAmpereTrip >= value);
    }

    public static int GetAmpereFrame(int ampereTrip)
    {
        return DataConstants.StandardAmpereFrameRatings.First(ampereFrame => ampereFrame >= ampereTrip);
    }

    public static double GetMotorOutlet230VoltAmpereLoad(double horsepower)
    {
        return DataConstants.MotorOutlet230VoltAmpereLoadRatings[
            MotorOutletCircuit.AllowedHorsepowerValues.FindIndex(hp => hp.IsRoughlyEqualTo(horsepower))];
    }
}