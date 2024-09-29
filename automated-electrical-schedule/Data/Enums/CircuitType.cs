using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum CircuitType
{
    [Display(Name = "Lighting Outlet")] LightingOutlet,
    [Display(Name = "Motor Outlet")] MotorOutlet,
    [Display(Name = "Convenience Outlet")] ConvenienceOutlet,

    [Display(Name = "Appliance/Equipment Outlet")]
    ApplianceEquipmentOutlet
}