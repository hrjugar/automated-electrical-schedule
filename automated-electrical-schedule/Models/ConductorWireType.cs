using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Models;

public enum ConductorWireType
{
    [Display(Name = "FEP")] Fep,
    [Display(Name = "FEPB")] Fepb,
    [Display(Name = "MI")] Mi,
    [Display(Name = "RHH")] Rhh,
    [Display(Name = "RHH-2")] Rhh2,
    [Display(Name = "RHW")] Rhw,
    [Display(Name = "RHW-2")] Rhw2,
    [Display(Name = "SA")] Sa,
    [Display(Name = "SIS")] Sis,
    [Display(Name = "TBS")] Tbs,
    [Display(Name = "THHN")] Thhn,
    [Display(Name = "THHW")] Thhw,
    [Display(Name = "THW")] Thw,
    [Display(Name = "THW-2")] Thw2,
    [Display(Name = "THWN")] Thwn,
    [Display(Name = "THWN-2")] Thwn2,
    [Display(Name = "TW")] Tw,
    [Display(Name = "UF")] Uf,
    [Display(Name = "USE")] Use,
    [Display(Name = "USE-2")] Use2,
    [Display(Name = "XHH")] Xhh,
    [Display(Name = "XHHW")] Xhhw,
    [Display(Name = "XHHW-2")] Xhhw2,
    [Display(Name = "ZW")] Zw,
    [Display(Name = "ZW-2")] Zw2
}