using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class FixtureCircuit : Circuit
{
    [Required]
    [Display(Name = "itemized")]
    [Column("is_itemized")]
    public bool IsItemized { get; set; } = false;
    
    [ValidateComplexType]
    public List<Fixture> Fixtures { get; init; } = [];
}