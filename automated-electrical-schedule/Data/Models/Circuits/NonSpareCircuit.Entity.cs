using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class NonSpareCircuit : NonSpaceCircuit, IDescribed
{
    [Required]
    [Display(Name = "description")]
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "wire length")]
    [Column("wire_length")]
    [Range(0d, double.PositiveInfinity, MinimumIsExclusive = true)]
    public override double WireLength { get; set; } = 1;
    
    [Required]
    [Display(Name = "conductor type")]
    [Column("conductor_type_id")]
    public override string ConductorTypeId { get; set; } = Records.ConductorType.All[0].Id;
    
    [Required]
    [Display(Name = "grounding")]
    [Column("grounding_id")]
    public override string GroundingId { get; set; } = Records.ConductorType.All[0].Id;
    
    [Required]
    [Display(Name = "raceway type")]
    [Column("raceway_type")]
    public override RacewayType RacewayType { get; set; } = RacewayType.Pvc;
}