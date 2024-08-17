using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Enums;
using Microsoft.EntityFrameworkCore;

namespace automated_electrical_schedule.Models;

[Table(TableName)]
public class DistributionBoard
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
    [Display(Name = "phase")]
    [Column("phase")]
    public BoardPhase Phase { get; set; }

    [Required]
    [Display(Name = "voltage")]
    [Column("voltage")]
    public BoardVoltage Voltage { get; set; }

    [Required]
    [Display(Name = "sets")]
    [Column("set_count")]
    public int SetCount { get; set; }

    [Required]
    [Display(Name = "conductor type")]
    [Column("conductor_type_id")]
    public int ConductorTypeId { get; set; }

    [ForeignKey(nameof(ConductorTypeId))] public ConductorType ConductorType { get; set; } = null!;

    [Required]
    [Display(Name = "grounding")]
    [Column("grounding_id")]
    public int GroundingId { get; set; }

    [ForeignKey(nameof(GroundingId))] public ConductorType Grounding { get; set; } = null!;

    [Required]
    [Display(Name = "raceway type")]
    [Column("raceway_type")]
    public RacewayType RacewayType { get; set; }
}