using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace automated_electrical_schedule.Data.Models;

[Table(TableName)]
public abstract partial class Circuit
{
    private const string TableName = "circuits";

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("parent_distribution_board_id")]
    public int ParentDistributionBoardId { get; set; }

    // TODO: Check later if cascade delete works as intended
    [ForeignKey(nameof(ParentDistributionBoardId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public DistributionBoard ParentDistributionBoard { get; set; } = null!;

    [Required]
    [Display(Name = "circuit type")]
    [Column("circuit_type")]
    public CircuitType CircuitType { get; set; }
    
    [Display(Name = "line-to-line voltage")]
    [Column("line_to_line_voltage")]
    public LineToLineVoltage? LineToLineVoltage { get; set; } = null;

    [Required]
    [Display(Name = "description")]
    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Display(Name = "quantity")]
    [Column("quantity")]
    [Range(1, int.MaxValue, ErrorMessage = "The quantity must be at least 1.")]
    public int Quantity { get; set; } = 1;

    [Required]
    [Display(Name = "wire length")]
    [Column("wire_length")]
    [Range(0d, double.PositiveInfinity, MinimumIsExclusive = true)]
    public double WireLength { get; set; } = 1;

    [Required]
    [Display(Name = "demand factor")]
    [Column("demand_factor")]
    [Range(0d, 100d, ErrorMessage = "The demand factor must be between 0 and 100 percent.")]
    public double DemandFactor { get; set; } = 100;

    // TODO: Limit allowed circuit protections depending on circuit type
    [Required]
    [Display(Name = "circuit protection")]
    [Column("circuit_protection")]
    public CircuitProtection CircuitProtection { get; set; } = CircuitProtection.MiniatureCircuitBreaker;

    [Required]
    [Display(Name = "sets")]
    [Column("set_count")]
    [Range(1, int.MaxValue, ErrorMessage = "The quantity must be at least 1.")]
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
    public RacewayType RacewayType { get; set; } = RacewayType.Pvc;
}