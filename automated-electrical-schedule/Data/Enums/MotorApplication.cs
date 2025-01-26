using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum MotorApplication
{
    [Display(Name = "Normal Motor Application")] NormalMotor,
    [Display(Name = "Elevator Feeder")] ElevatorFeeder,
    [Display(Name = "Cranes and Hoist")] CranesAndHoist
}