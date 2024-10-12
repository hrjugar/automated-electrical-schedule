using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data;

public static class DataUtils
{
    public static int GetAmpereTrip(double value, int minimumAmpereTrip = 0)
    {
        if (value == 0) return 0;

        return DataConstants.StandardAmpereTripRatings
            .First(columnAmpereTrip => columnAmpereTrip >= minimumAmpereTrip && columnAmpereTrip >= value);
    }

    public static int GetAmpereFrame(int ampereTrip)
    {
        if (ampereTrip == 0) return 0;

        return DataConstants.StandardAmpereFrameRatings.First(ampereFrame => ampereFrame >= ampereTrip);
    }

    public static double GetMotorOutlet230VoltAmpereLoad(double horsepower)
    {
        return DataConstants.SinglePhaseMotorOutletAmpereLoadRatings[
            DataConstants.SinglePhaseHorsepowerValues.FindIndex(hp => hp.IsRoughlyEqualTo(horsepower))];
    }
}