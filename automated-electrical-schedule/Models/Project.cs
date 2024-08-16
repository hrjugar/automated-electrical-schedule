using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Models;

public class Project
{
    [Required] public DistributionBoard MainDistributionBoard = new();

    [Required]
    [Display(Name = "project name")]
    public string ProjectName { get; set; } = "";
}