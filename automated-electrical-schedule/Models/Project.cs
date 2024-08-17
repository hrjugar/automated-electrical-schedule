using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace automated_electrical_schedule.Models;

[Table(TableName)]
public class Project
{
    private const string TableName = "projects";

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("main_distribution_board_id")]
    public int MainDistributionBoardId { get; set; }

    [ForeignKey(nameof(MainDistributionBoardId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public DistributionBoard MainDistributionBoard { get; set; } = null!;

    [Required]
    [Display(Name = "project name")]
    [Column("project_name")]
    [MaxLength(255)]
    public string ProjectName { get; set; } = string.Empty;
}