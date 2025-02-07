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

    [Display(Name = "wire length")]
    [Column("wire_length")]
    [Range(0d, double.PositiveInfinity)]
    public override double WireLength { get; set; } = 0;
}