using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Records;
using Microsoft.EntityFrameworkCore;

namespace automated_electrical_schedule.Data.Models;

[Table(TableName)]
public abstract partial class Circuit : IElectricalComponent
{
    private const string TableName = "circuits";

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("parent_distribution_board_id")]
    public int ParentDistributionBoardId { get; set; }

    [Required]
    [ForeignKey(nameof(ParentDistributionBoardId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public DistributionBoard ParentDistributionBoard { get; set; } = default!;
    
    [Required]
    [Column("order")]
    public int Order { get; set; }
    
    [Required]
    [Display(Name = "circuit type")]
    [Column("circuit_type")]
    public CircuitType CircuitType { get; set; }
    
    [Display(Name = "line-to-line voltage")]
    [Column("line_to_line_voltage")]
    public LineToLineVoltage LineToLineVoltage { get; set; } = LineToLineVoltage.None;

    [Required]
    [Display(Name = "description")]
    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Display(Name = "wire length")]
    [Column("wire_length")]
    [Range(0d, double.PositiveInfinity, MinimumIsExclusive = true)]
    public double WireLength { get; set; } = 1;

    // TODO: Limit allowed circuit protections depending on circuit type
    [Required]
    [Display(Name = "circuit protection")]
    [Column("circuit_protection")]
    public CircuitProtection CircuitProtection { get; set; } = CircuitProtection.MiniatureCircuitBreaker;

    [Required]
    [Display(Name = "sets")]
    [Column("set_count")]
    [Range(1, int.MaxValue, ErrorMessage = "The set count must be at least 1.")]
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
    public RacewayType RacewayType { get; set; } = RacewayType.Pvc;
}