using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum OutletType
{
    [Display(Name = "1-Gang")] OneGang = 1,
    [Display(Name = "2-Gang")] TwoGang = 2,
    [Display(Name = "3-Gang")] ThreeGang = 3,
    [Display(Name = "4-Gang")] FourGang = 4
}