using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class MotorOutletCircuit : Circuit
{
    [Required]
    [Display(Name = "motor type")]
    [Column("motor_type")]
    public MotorType MotorType { get; set; } = MotorType.SinglePhaseMotor;

    [Required]
    [Display(Name = "horsepower")]
    [Column("horsepower")]
    [Range(0d, double.PositiveInfinity, MinimumIsExclusive = true)]
    public double Horsepower { get; set; }
}