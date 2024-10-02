using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace automated_electrical_schedule.Data.Models;

public partial class ApplianceEquipmentOutletCircuit : Circuit
{
    [Required]
    [Display(Name = "wattage")]
    [Column("wattage")]
    [Range(0d, double.PositiveInfinity)]
    public double Wattage { get; set; }
}