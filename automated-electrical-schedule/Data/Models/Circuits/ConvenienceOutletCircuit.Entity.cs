using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Validators;

namespace automated_electrical_schedule.Data.Models;

public partial class ConvenienceOutletCircuit : Circuit
{
    [Required]
    [Display(Name = "GFCI Receptacle quantity")]
    [Column("gfci_receptacle_quantity")]
    public int GfciReceptacleQuantity { get; set; }
    
    [Required]
    [Display(Name = "GFCI Receptacle yoke")]
    [Column("gfci_receptacle_yoke")]
    [ConvenienceGangValidator(nameof(GfciReceptacleQuantity), 180)]
    public double GfciReceptacleYoke { get; set; }
    
    [Required]
    [Display(Name = "1-gang quantity")]
    [Column("one_gang_quantity")]
    
    public int OneGangQuantity { get; set; }
    
    [Required]
    [Display(Name = "1-gang yoke")]
    [Column("one_gang_yoke")]
    [ConvenienceGangValidator(nameof(OneGangQuantity), 180)]
    public double OneGangYoke { get; set; }
    
    [Required]
    [Display(Name = "2-gang quantity")]
    [Column("two_gang_quantity")]
    public int TwoGangQuantity { get; set; }
    
    [Required]
    [Display(Name = "2-gang yoke")]
    [Column("two_gang_yoke")]
    [ConvenienceGangValidator(nameof(TwoGangQuantity), 180)]
    public double TwoGangYoke { get; set; }
    
    [Required]
    [Display(Name = "3-gang quantity")]
    [Column("three_gang_quantity")]
    public int ThreeGangQuantity { get; set; }
    
    [Required]
    [Display(Name = "3-gang yoke")]
    [Column("three_gang_yoke")]
    [ConvenienceGangValidator(nameof(ThreeGangQuantity), 180)]
    public double ThreeGangYoke { get; set; }
    
    [Required]
    [Display(Name = "4-gang quantity")]
    [Column("four_gang_quantity")]
    public int FourGangQuantity { get; set; }
    
    [Required]
    [Display(Name = "4-gang yoke")]
    [Column("four_gang_yoke")]
    [ConvenienceGangValidator(nameof(FourGangQuantity), 360)]
    public double FourGangYoke { get; set; }
}