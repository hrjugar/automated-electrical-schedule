using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum BoardPhase
{
    [Display(Name = "Single Phase")] SinglePhase,
    [Display(Name = "Three Phase")] ThreePhase
}