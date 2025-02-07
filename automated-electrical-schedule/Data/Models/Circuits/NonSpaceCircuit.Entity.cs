using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class NonSpaceCircuit : Circuit, IElectricalComponent
{
    public abstract double WireLength { get; set; }

    [Display(Name = "line-to-line voltage")]
    [Column("line_to_line_voltage")]
    public LineToLineVoltage LineToLineVoltage { get; set; } = LineToLineVoltage.None;
    
    
    [Required]
    [Display(Name = "circuit protection")]
    [Column("circuit_protection")]
    public CircuitProtection CircuitProtection { get; set; } = CircuitProtection.MiniatureCircuitBreaker;
    
    [Required]
    [Display(Name = "sets")]
    [Column("set_count")]
    [Range(1, int.MaxValue, ErrorMessage = "The set count must be at least 1.")]
    public int SetCount { get; set; } = 1;
    
    [Required]
    [Display(Name = "conductor type")]
    [Column("conductor_type_id")]
    public string ConductorTypeId { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "grounding")]
    [Column("grounding_id")]
    public string GroundingId { get; set; } = string.Empty;

    [Required]
    [Display(Name = "raceway type")]
    [Column("raceway_type")]
    public RacewayType RacewayType { get; set; } = RacewayType.None;
}