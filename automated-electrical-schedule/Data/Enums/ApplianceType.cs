using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum ApplianceType
{
    [Display(Name = "Clothes Dryer")] Dryer,
    [Display(Name = "Kitchen Equipment")] KitchenEquipment,
    [Display(Name = "Other")] Other
}