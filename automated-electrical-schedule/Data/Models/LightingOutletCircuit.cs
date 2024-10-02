using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace automated_electrical_schedule.Data.Models;

public partial class LightingOutletCircuit : Circuit
{
    [Required]
    [Display(Name = "wattage per fixture")]
    [Column("wattage_per_fixture")]
    [Range(0d, double.PositiveInfinity)]
    public double WattagePerFixture { get; set; }
}