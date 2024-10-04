using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class ThreePhaseDistributionBoard : DistributionBoard
{
    [Required]
    [Display(Name = "three phase configuration")]
    [Column("three_phase_configuration")]
    public ThreePhaseConfiguration ThreePhaseConfiguration { get; set; }

    [Display(Name = "transformer primary protection")]
    [Column("transformer_primary_protection")]
    public CircuitProtection? TransformerPrimaryProtection { get; set; }

    [Display(Name = "transformer secondary protection")]
    [Column("transformer_secondary_protection")]
    public CircuitProtection? TransformerSecondaryProtection { get; set; }


    /* USE ONLY FOR SUB BOARDS */
    [Display(Name = "line to line voltage")]
    [Column("line_to_line_voltage")]
    public LineToLineVoltage? LineToLineVoltage { get; set; } = null;

    /* USE ONLY WHEN STEPPING DOWN */

    [Display(Name = "breaker circuit protection")]
    [Column("breaker_circuit_protection")]
    public CircuitProtection? BreakerCircuitProtection { get; set; }

    [Display(Name = "breaker sets")]
    [Column("breaker_set_count")]
    public int? BreakerSetCount { get; set; }

    [Display(Name = "breaker conductor type")]
    [Column("breaker_conductor_type_id")]
    public int? BreakerConductorTypeId { get; set; }

    [ForeignKey(nameof(BreakerConductorTypeId))]
    [ValidateComplexType]
    public ConductorType? BreakerConductorType { get; set; }

    [Display(Name = "breaker grounding")]
    [Column("breaker_grounding_id")]
    public int? BreakerGroundingId { get; set; }

    [ForeignKey(nameof(BreakerGroundingId))]
    [ValidateComplexType]
    public ConductorType? BreakerGrounding { get; set; }

    [Display(Name = "breaker raceway type")]
    [Column("breaker_raceway_type")]
    public RacewayType? BreakerRacewayType { get; set; }
}