using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Models;

public enum ProjectPhase
{
    [Display(Name = "Single Phase")] SinglePhase,

    [Display(Name = "Three Phase - Delta")]
    ThreePhaseDelta,

    [Display(Name = "Three Phase - WYE")] ThreePhaseWye
}

public enum ProjectVoltage
{
    [Display(Name = "230 Volts")] V230,

    [Display(Name = "400 Volts")] V400,

    [Display(Name = "460 Volts")] V460,

    [Display(Name = "575 Volts")] V575
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