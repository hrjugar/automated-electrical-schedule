using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum BoardVoltage
{
    [Display(Name = "230V")] V230 = 230,

    [Display(Name = "400V 3Φ / 230V 1Φ")] V400 = 400,

    [Display(Name = "460V")] V460 = 460,

    [Display(Name = "575V")] V575 = 575
}