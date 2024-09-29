using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public class ConvenienceOutletCircuit : Circuit
{
    [Required]
    [Display(Name = "outlet type")]
    [Column("outlet_type")]
    public OutletType OutletType { get; set; } = OutletType.OneGang;
}