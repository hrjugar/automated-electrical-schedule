using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Enums;

public enum CalculationErrorType
{
    [Display(Name = "No error")] NoError,
    [Display(Name = "Cannot calculate without circuits")] NoCircuits,
    [Display(Name = "Cannot calculate without transformer")] NoTransformer,
    [Display(Name = "No current in distribution board")] NoBoardCurrent,
    [Display(Name = "Ampere trip exceeds maximum possible rating")] NoFittingAmpereTrip,
    [Display(Name = "No fitting ampere trip for conductor size")] NoFittingAmpereTripForConductorSize,
    [Display(Name = "No fitting ampere trip for grounding size")] NoFittingAmpereTripForGroundingSize,
    [Display(Name = "Ampere frame exceeds maximum possible rating")] NoFittingAmpereFrame,
    [Display(Name = "Conductor size exceeds maximum possible size")] NoFittingConductorSize,
    [Display(Name = "Raceway size exceeds maximum possible size")] NoFittingRacewaySize,
    [Display(Name = "Horsepower exceeds maximum possible value")] NoFittingHorsepower,
    [Display(Name = "Fill area exceeds maximum possible area")] NoFittingFillArea,
    [Display(Name = "Transformer rating exceeds maximum possible rating")] NoFittingTransformerRating,
    [Display(Name = "Invalid raceway type used")] InvalidRacewayType,
    [Display(Name = "Invalid conductor material used")] InvalidConductorMaterial,
    [Display(Name = "Invalid conductor temperature rating used")] InvalidConductorTemperatureRating,
    [Display(Name = "Invalid conductor wire type used")] InvalidConductorWireType,
    [Display(Name = "Invalid motor type used")] InvalidMotorType,
    [Display(Name = "Invalid circuit protection used")] InvalidCircuitProtection,
}