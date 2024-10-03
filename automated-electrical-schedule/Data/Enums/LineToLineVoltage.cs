using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum LineToLineVoltage
{
    [Display(Name = "AB")] Ab,
    [Display(Name = "BC")] Bc,
    [Display(Name = "CA")] Ca,
    [Display(Name = "ABC")] Abc
}