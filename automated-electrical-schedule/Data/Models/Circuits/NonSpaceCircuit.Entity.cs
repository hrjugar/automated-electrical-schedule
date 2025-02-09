using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class NonSpaceCircuit : Circuit, IElectricalComponent
{
    
    public abstract string ConductorTypeId { get; set; }
    
    public abstract string GroundingId { get; set; }
    
    public abstract RacewayType RacewayType { get; set; }
    
    [Required]
    [Display(Name = "circuit protection")]
    [Column("circuit_protection")]
    public CircuitProtection CircuitProtection { get; set; } = CircuitProtection.MiniatureCircuitBreaker;
    
    [Required]
    [Display(Name = "sets")]
    [Column("set_count")]
    [Range(1, int.MaxValue, ErrorMessage = "The set count must be at least 1.")]
    public int SetCount { get; set; } = 1;
    
    [Display(Name = "wire length")]
    [Column("wire_length")]
    [Range(0d, double.PositiveInfinity, MinimumIsExclusive = true)]
    public double? WireLength { get; set; }
    
    [Column("voltage_drop_correction_conductor_size")]
    public double? VoltageDropCorrectionConductorSize { get; set; }
}