using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class ThreePhaseDistributionBoard : DistributionBoard
{
    [Required]
    [Display(Name = "three phase configuration")]
    [Column("three_phase_configuration")]
    public ThreePhaseConfiguration ThreePhaseConfiguration { get; set; }
}