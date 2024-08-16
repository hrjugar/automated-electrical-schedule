using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Models;

public enum ConductorTemperatureRating
{
    [Display(Name = "60°C")] C60 = 60,
    [Display(Name = "75°C")] C75 = 75,
    [Display(Name = "90°C")] C90 = 90
}