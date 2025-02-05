using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum AmbientTemperature
{
    [Display(Name = "10 or less")]
    TenOrLess,
    
    [Display(Name = "11-15")]
    ElevenToFifteen,
    
    [Display(Name = "16-20")]
    SixteenToTwenty,
    
    [Display(Name = "21-25")]
    TwentyOneToTwentyFive,
    
    [Display(Name = "26-30")]
    TwentySixToThirty,
    
    [Display(Name = "31-35")]
    ThirtyOneToThirtyFive,
    
    [Display(Name = "36-40")]
    ThirtySixToForty,
    
    [Display(Name = "41-45")]
    FortyOneToFortyFive,
    
    [Display(Name = "46-50")]
    FortySixToFifty,
    
    [Display(Name = "51-55")]
    FiftyOneToFiftyFive,
    
    [Display(Name = "56-60")]
    FiftySixToSixty,
    
    [Display(Name = "61-65")]
    SixtyOneToSixtyFive,
    
    [Display(Name = "66-70")]
    SixtySixToSeventy,
    
    [Display(Name = "71-75")]
    SeventyOneToSeventyFive,
    
    [Display(Name = "76-80")]
    SeventySixToEighty,
    
    [Display(Name = "81-85")]
    EightyOneToEightyFive,
}