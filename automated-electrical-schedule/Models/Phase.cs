using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Models;

public enum Phase
{
    [Display(Name = "Single Phase")] SinglePhase,

    [Display(Name = "Three Phase - Delta")]
    ThreePhaseDelta,

    [Display(Name = "Three Phase - WYE")] ThreePhaseWye
}