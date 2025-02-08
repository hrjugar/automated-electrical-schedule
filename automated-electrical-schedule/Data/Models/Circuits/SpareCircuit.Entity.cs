using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Wrappers;

namespace automated_electrical_schedule.Data.Models;

public partial class SpareCircuit : NonSpaceCircuit
{
    [Required]
    [Display(Name = "volt ampere")]
    [Column("volt_ampere")]
    [Range(0d, double.MaxValue)]
    public double VoltAmpere { get; set; }
    
    [Display(Name = "conductor type")]
    [Column("conductor_type_id")]
    public override string ConductorTypeId { get; set; } = string.Empty;
    
    [Display(Name = "grounding")]
    [Column("grounding_id")]
    public override string GroundingId { get; set; } = string.Empty;
    
    [Display(Name = "raceway type")]
    [Column("raceway_type")]
    public override RacewayType RacewayType { get; set; } = RacewayType.None;
}