using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class MotorOutletCircuit : NonSpareCircuit
{
    [Required]
    [Display(Name = "motor type")]
    [Column("motor_type")]
    public MotorType MotorType { get; set; }

    [Required]
    [Display(Name = "motor application")]
    [Column("motor_application")]
    public MotorApplication MotorApplication { get; set; }
    
    [Required]
    [Display(Name = "horsepower")]
    [Column("horsepower")]
    public string Horsepower { get; set; }
}