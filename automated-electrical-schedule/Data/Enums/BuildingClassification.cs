using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum BuildingClassification
{
    [Display(Name = "Dwelling Unit")] DwellingUnit,
    [Display(Name = "Hospital")] Hospital,
    [Display(Name = "Hotel/Motel/Apartment")] HotelMotelApartment,
    [Display(Name = "Warehouses (Storage)")] Warehouse,
    [Display(Name = "Other")] Other,
    [Display(Name = "None")] None
}