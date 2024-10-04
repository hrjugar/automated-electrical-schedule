using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum ThreePhaseConfiguration
{
    [Display(Name = "Delta")] Delta,
    [Display(Name = "WYE")] Wye
}