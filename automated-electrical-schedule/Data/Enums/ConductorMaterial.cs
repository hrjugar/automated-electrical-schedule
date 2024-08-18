using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum ConductorMaterial
{
    [Display(Name = "Al")] Aluminum,
    [Display(Name = "Cu")] Copper
}