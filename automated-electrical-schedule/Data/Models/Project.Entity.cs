using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Validators;
using Microsoft.EntityFrameworkCore;

namespace automated_electrical_schedule.Data.Models;

[Table(TableName)]
public class Project
{
    private const string TableName = "projects";

    public Project()
    {
    }

    public Project(DistributionBoard mainDistributionBoard)
    {
        MainDistributionBoard = mainDistributionBoard;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("main_distribution_board_id")]
    public int MainDistributionBoardId { get; set; }

    [Required]
    [ForeignKey(nameof(MainDistributionBoardId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    [ValidateComplexType]
    public DistributionBoard MainDistributionBoard { get; set; } = null!;

    [Required]
    [Display(Name = "project name")]
    [Column("project_name")]
    [MaxLength(255)]
    [ProjectNameValidator]
    public string ProjectName { get; set; } = string.Empty;

    [Column("date_created")]
    [MaxLength(255)]
    public string DateCreated { get; set; } = string.Empty;
}