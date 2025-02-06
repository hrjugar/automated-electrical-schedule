using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class ApplianceEquipmentOutletCircuit : FixtureCircuit
{
    [Required]
    [Display(Name = "appliance type")]
    [Column("appliance_type")]
    public ApplianceType ApplianceType { get; set; } = ApplianceType.Other;
}