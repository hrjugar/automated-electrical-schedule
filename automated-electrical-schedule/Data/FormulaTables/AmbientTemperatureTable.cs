using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.FormulaTables;

public static class AmbientTemperatureTable
{
    private static Dictionary<AmbientTemperature, double> SixtyDegreesTemperatureRatings = new()
    {
        { AmbientTemperature.TenOrLess, 1.29 },
        { AmbientTemperature.ElevenToFifteen, 1.22 },
        { AmbientTemperature.SixteenToTwenty, 1.15 },
        { AmbientTemperature.TwentyOneToTwentyFive, 1.08 },
        { AmbientTemperature.TwentySixToThirty, 1 },
        { AmbientTemperature.ThirtyOneToThirtyFive, 0.91 },
        { AmbientTemperature.ThirtySixToForty, 0.82 },
        { AmbientTemperature.FortyOneToFortyFive, 0.71 },
        { AmbientTemperature.FortySixToFifty, 0.50 },
        { AmbientTemperature.FiftyOneToFiftyFive, 0.41 }
    };
    
    private static Dictionary<AmbientTemperature, double> SeventyFiveDegreesTemperatureRatings = new()
    {
        { AmbientTemperature.TenOrLess, 1.2 },
        { AmbientTemperature.ElevenToFifteen, 1.15 },
        { AmbientTemperature.SixteenToTwenty, 1.11 },
        { AmbientTemperature.TwentyOneToTwentyFive, 1.05 },
        { AmbientTemperature.TwentySixToThirty, 1 },
        { AmbientTemperature.ThirtyOneToThirtyFive, 0.94 },
        { AmbientTemperature.ThirtySixToForty, 0.88 },
        { AmbientTemperature.FortyOneToFortyFive, 0.75 },
        { AmbientTemperature.FortySixToFifty, 0.67 },
        { AmbientTemperature.FiftyOneToFiftyFive, 0.58 },
        { AmbientTemperature.FiftySixToSixty, 0.47 },
        { AmbientTemperature.SixtyOneToSixtyFive, 0.33 }
    };
    
    private static Dictionary<AmbientTemperature, double> NinetyDegreesTemperatureRatings = new()
    {
        { AmbientTemperature.TenOrLess, 1.15 },
        { AmbientTemperature.ElevenToFifteen, 1.12 },
        { AmbientTemperature.SixteenToTwenty, 1.08 },
        { AmbientTemperature.TwentyOneToTwentyFive, 1.04 },
        { AmbientTemperature.TwentySixToThirty, 1 },
        { AmbientTemperature.ThirtyOneToThirtyFive, 0.96 },
        { AmbientTemperature.ThirtySixToForty, 0.91 },
        { AmbientTemperature.FortyOneToFortyFive, 0.87 },
        { AmbientTemperature.FortySixToFifty, 0.82 },
        { AmbientTemperature.FiftyOneToFiftyFive, 0.76 },
        { AmbientTemperature.FiftySixToSixty, 0.71 },
        { AmbientTemperature.SixtyOneToSixtyFive, 0.65 },
        { AmbientTemperature.SixtySixToSeventy, 0.58 },
        { AmbientTemperature.SeventyOneToSeventyFive, 0.5 },
        { AmbientTemperature.SeventySixToEighty, 0.41 },
        { AmbientTemperature.EightyOneToEightyFive, 0.29 }
    };

    public static double GetAmbientTemperatureMultiplier(AmbientTemperature ambientTemperature, ConductorTemperatureRating conductorTemperatureRating)
    {
        var temperatureRatingDict = conductorTemperatureRating switch
        {
            ConductorTemperatureRating.C60 => SixtyDegreesTemperatureRatings,
            ConductorTemperatureRating.C75 => SeventyFiveDegreesTemperatureRatings,
            ConductorTemperatureRating.C90 => NinetyDegreesTemperatureRatings,
            _ => throw new ArgumentOutOfRangeException(nameof(conductorTemperatureRating), conductorTemperatureRating, null)
        };

        return temperatureRatingDict.TryGetValue(ambientTemperature, out var value) ? value : 1;
    }
}