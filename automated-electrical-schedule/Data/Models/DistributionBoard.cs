using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace automated_electrical_schedule.Data.Models;

[Table(TableName)]
public abstract partial class DistributionBoard
{
    private const string TableName = "distribution_boards";

    public static readonly List<CircuitProtection> AllowedCircuitProtections =
    [
        CircuitProtection.MiniatureCircuitBreaker,
        CircuitProtection.MoldedCaseCircuitBreaker,
        CircuitProtection.OilCircuitBreaker,
        CircuitProtection.AirBreakCircuitBreaker,
        CircuitProtection.AirBlastCircuitBreaker,
        CircuitProtection.VacuumCircuitBreaker,
        CircuitProtection.SulfurHexafluorideCircuitBreaker
    ];

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Display(Name = "board name")]
    [Column("name")]
    [MaxLength(255)]
    public string BoardName { get; set; } = string.Empty;

    [Column("parent_distribution_board_id")]
    public int? ParentDistributionBoardId { get; set; }

    // TODO: Check later if cascade delete works as intended
    [ForeignKey(nameof(ParentDistributionBoardId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public DistributionBoard? ParentDistributionBoard { get; set; }

    [Required]
    [Display(Name = "phase")]
    [Column("phase")]
    public BoardPhase Phase { get; set; } = BoardPhase.SinglePhase;

    [Required]
    [Display(Name = "voltage")]
    [Column("voltage")]
    public BoardVoltage Voltage { get; set; } = BoardVoltage.V230;

    [Display(Name = "wire length")]
    [Column("wire_length")]
    [Range(0d, double.PositiveInfinity, MinimumIsExclusive = true)]
    public double? WireLength { get; set; }

    [Required]
    [Display(Name = "circuit protection")]
    [Column("circuit_protection")]
    public CircuitProtection CircuitProtection { get; set; } = CircuitProtection.MiniatureCircuitBreaker;

    [Display(Name = "line-to-line voltage")]
    [Column("line_to_line_voltage")]
    public LineToLineVoltage? LineToLineVoltage { get; set; } = null;

    [Required]
    [Display(Name = "sets")]
    [Column("set_count")]
    [Range(1, 100, ErrorMessage = "The number of sets must be between 1 and 100.")]
    public int SetCount { get; set; } = 1;

    [Required]
    [Display(Name = "conductor type")]
    [Column("conductor_type_id")]
    public int ConductorTypeId { get; set; }

    [ForeignKey(nameof(ConductorTypeId))]
    [ValidateComplexType]
    public ConductorType ConductorType { get; set; } = null!;

    [Required]
    [Display(Name = "grounding")]
    [Column("grounding_id")]
    public int GroundingId { get; set; }

    [ForeignKey(nameof(GroundingId))]
    [ValidateComplexType]
    public ConductorType Grounding { get; set; } = null!;

    [Required]
    [Display(Name = "raceway type")]
    [Column("raceway_type")]
    public RacewayType RacewayType { get; set; }

    /* USE WHEN THIS OR PARENT BOARD IS THREE PHASE */

    [Display(Name = "transformer primary protection")]
    [Column("transformer_primary_protection")]
    public CircuitProtection? TransformerPrimaryProtection { get; set; }

    [Display(Name = "transformer secondary protection")]
    [Column("transformer_secondary_protection")]
    public CircuitProtection? TransformerSecondaryProtection { get; set; }

    /* USE ONLY WHEN STEPPING DOWN */

    [Display(Name = "breaker circuit protection")]
    [Column("breaker_circuit_protection")]
    public CircuitProtection? BreakerCircuitProtection { get; set; }

    [Display(Name = "breaker sets")]
    [Column("breaker_set_count")]
    public int? BreakerSetCount { get; set; }

    [Display(Name = "breaker conductor type")]
    [Column("breaker_conductor_type_id")]
    public int? BreakerConductorTypeId { get; set; }

    [ForeignKey(nameof(BreakerConductorTypeId))]
    [ValidateComplexType]
    public ConductorType? BreakerConductorType { get; set; }

    [Display(Name = "breaker grounding")]
    [Column("breaker_grounding_id")]
    public int? BreakerGroundingId { get; set; }

    [ForeignKey(nameof(BreakerGroundingId))]
    [ValidateComplexType]
    public ConductorType? BreakerGrounding { get; set; }

    [Display(Name = "breaker raceway type")]
    [Column("breaker_raceway_type")]
    public RacewayType? BreakerRacewayType { get; set; }

    /* LISTS OF REFERENCES */

    public List<DistributionBoard> SubDistributionBoards { get; set; } = [];

    public List<Circuit> Circuits { get; } = [];
}