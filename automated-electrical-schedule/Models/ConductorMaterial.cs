using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Models;

public enum ConductorMaterial
{
    [Display(Name = "Al")] Aluminum,
    [Display(Name = "Cu")] Copper
}