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
}