using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum RacewayType
{
    [Display(Name = "PVC")] Pvc,

    [Display(Name = "EMT")] Emt,

    [Display(Name = "ENT")] Ent,

    [Display(Name = "FMC")] Fmc,

    [Display(Name = "IMC")] Imc,

    [Display(Name = "RMC")] Rmc,

    [Display(Name = "Cable Tray")] CableTray
}