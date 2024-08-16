using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Models;

public class DistributionBoard(
    string boardName,
    DistributionBoard? parentDistributionBoard,
    Phase phase,
    Voltage voltage,
    int setCount,
    ConductorType conductorType,
    ConductorType grounding,
    RacewayType racewayType
)
{
    public DistributionBoard() : this(
        "",
        null,
        Phase.SinglePhase,
        Voltage.V230,
        1,
        ConductorType.ThhnCu90,
        ConductorType.TwCu60,
        RacewayType.Pvc
    )
    {
    }

    [Required]
    [Display(Name = "board name")]
    public string BoardName { get; set; } = boardName;

    public DistributionBoard? ParentDistributionBoard { get; set; } = parentDistributionBoard;

    [Required] [Display(Name = "phase")] public Phase Phase { get; set; } = phase;

    [Required] [Display(Name = "voltage")] public Voltage Voltage { get; set; } = voltage;

    [Required] [Display(Name = "sets")] public int SetCount { get; set; } = setCount;

    [Required]
    [Display(Name = "conductor type")]
    public ConductorType ConductorType { get; set; } = conductorType;

    [Required]
    [Display(Name = "grounding")]
    public ConductorType Grounding { get; set; } = grounding;

    [Required]
    [Display(Name = "raceway type")]
    public RacewayType RacewayType { get; set; } = racewayType;
}