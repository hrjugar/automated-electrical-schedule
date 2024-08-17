using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Enums;

public enum BoardPhase
{
    [Display(Name = "Single Phase")] SinglePhase,

    [Display(Name = "Three Phase - Delta")]
    ThreePhaseDelta,

    [Display(Name = "Three Phase - WYE")] ThreePhaseWye
}