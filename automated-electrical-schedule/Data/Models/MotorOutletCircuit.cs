using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class MotorOutletCircuit : Circuit
{
    public static readonly List<double> AllowedHorsepowerValues =
    [
        1.0 / 6,
        0.25,
        0.5,
        0.75,
        1.0,
        1.5,
        2.0,
        3.0,
        5.0,
        7.5,
        10
    ];

    [Required]
    [Display(Name = "motor description")]
    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Display(Name = "motor type")]
    [Column("motor_type")]
    public MotorType MotorType { get; set; } = MotorType.SinglePhaseMotor;

    [Required]
    [Display(Name = "horsepower")]
    [Column("horsepower")]
    [Range(0d, double.PositiveInfinity)]
    public double Horsepower { get; set; }
}