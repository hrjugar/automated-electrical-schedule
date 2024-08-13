using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Models;

public enum ProjectPhase
{
    SinglePhase,
    ThreePhaseDelta,
    ThreePhaseWye
}

public enum ProjectVoltage
{
    V230,
    V400,
    V460,
    V575
}

public class Project
{
    private ProjectPhase _phase = ProjectPhase.SinglePhase;

    [Required]
    [Display(Name = "project name")]
    public string ProjectName { get; set; } = "";

    [Required]
    [Display(Name = "distribution board name")]
    public string DistributionBoardName { get; set; } = "";

    public ProjectPhase Phase
    {
        get => _phase;
        set
        {
            _phase = value;
            if (value == ProjectPhase.SinglePhase) Voltage = ProjectVoltage.V230;
        }
    }

    public ProjectVoltage Voltage { get; set; } = ProjectVoltage.V230;
}