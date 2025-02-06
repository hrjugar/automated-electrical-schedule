using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace automated_electrical_schedule.Data.Models;

[Table(TableName)]
public abstract partial class DistributionBoard : IElectricalComponent
{
    private const string TableName = "distribution_boards";

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
    [Column("order")]
    public int Order { get; set; }
    
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
    public LineToLineVoltage LineToLineVoltage { get; set; } = LineToLineVoltage.None;

    [Required]
    [Display(Name = "sets")]
    [Column("set_count")]
    [Range(1, 100, ErrorMessage = "The number of sets must be between 1 and 100.")]
    public int SetCount { get; set; } = 1;

    [Required]
    [Display(Name = "conductor type")]
    [Column("conductor_type_id")]
    public string ConductorTypeId { get; set; } = ConductorType.All[0].Id;

    [Required]
    [Display(Name = "grounding")]
    [Column("grounding_id")]
    public string GroundingId { get; set; } = ConductorType.All[0].Id;

    [Required]
    [Display(Name = "raceway type")]
    [Column("raceway_type")]
    public RacewayType RacewayType { get; set; }
    
    [Required]
    [Display(Name = "building classification")]
    [Column("building_classification")]
    public BuildingClassification BuildingClassification { get; set; } = BuildingClassification.Other;

    [Required]
    [Display(Name = "ambient temperature")]
    [Column("ambient_temperature")]
    public AmbientTemperature AmbientTemperature { get; set; }
    
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
    public string? BreakerConductorTypeId { get; set; }

    [Display(Name = "breaker grounding")]
    [Column("breaker_grounding_id")]
    public string? BreakerGroundingId { get; set; }

    [Display(Name = "breaker raceway type")]
    [Column("breaker_raceway_type")]
    public RacewayType? BreakerRacewayType { get; set; }

    /* LISTS OF REFERENCES */

    public List<DistributionBoard> SubDistributionBoards { get; set; } = [];

    public List<Circuit> Circuits { get; } = [];
}