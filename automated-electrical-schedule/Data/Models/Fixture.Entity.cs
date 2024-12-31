using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace automated_electrical_schedule.Data.Models;

[Table(TableName)]
public partial class Fixture
{
    private const string TableName = "fixtures";
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("parent_outlet_id")]
    public int ParentCircuitId { get; set; }
    
    [Required]
    [ForeignKey(nameof(ParentCircuitId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public FixtureCircuit ParentCircuit { get; set; } = default!;
    
    [Display(Name = "description")]
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "quantity")]
    [Column("quantity")]
    [Range(1, int.MaxValue, ErrorMessage = "The quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Required]
    [Display(Name = "wattage")]
    [Column("wattage")]
    [Range(0d, double.PositiveInfinity, MinimumIsExclusive = true)]
    public double Wattage { get; set; }
}