using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum MotorType
{
    // TODO: Limit MotorType selection to only SinglePhaseMotor if Phase is Single Phase
    [Display(Name = "Single Phase Motor")] SinglePhaseMotor,
    [Display(Name = "Squirrel Cage")] SquirrelCage,

    [Display(Name = "Design B Energy Efficient")]
    DesignBEnergyEfficient,
    [Display(Name = "Synchronous")] Synchronous,
    [Display(Name = "Wound Rotor")] WoundRotor,

    [Display(Name = "DC (Constant Voltage)")]
    DcConstantVoltage
}