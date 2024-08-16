using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Models;

public enum Voltage
{
    [Display(Name = "230 Volts")] V230 = 230,

    [Display(Name = "400 Volts")] V400 = 400,

    [Display(Name = "460 Volts")] V460 = 460,

    [Display(Name = "575 Volts")] V575 = 575
}