using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum CircuitProtection
{
    [Display(Name = "Miniature Circuit Breaker")]
    MiniatureCircuitBreaker,

    [Display(Name = "Molded Case Circuit Breaker")]
    MoldedCaseCircuitBreaker,

    [Display(Name = "Oil Circuit Breaker")]
    OilCircuitBreaker,

    [Display(Name = "Air-Break Circuit Breaker")]
    AirBreakCircuitBreaker,

    [Display(Name = "Air-Blast Circuit Breaker")]
    AirBlastCircuitBreaker,

    [Display(Name = "Vacuum Circuit Breaker")]
    VacuumCircuitBreaker,

    [Display(Name = "Sulfur Hexafluoride Circuit Breaker")]
    SulfurHexafluorideCircuitBreaker,

    [Display(Name = "Non-Time Delay Fuse")]
    NonTimeDelayFuse,

    [Display(Name = "Dual Element (Time Delay Fuse)")]
    DualElement,

    [Display(Name = "Instantaneous Trip Breaker")]
    InstantaneousTripBreaker,

    [Display(Name = "Inverse Time Breaker")]
    InverseTimeBreaker
}