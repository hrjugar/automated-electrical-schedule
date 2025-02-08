using System.ComponentModel.DataAnnotations;
using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;

namespace automated_electrical_schedule.Data.Validators;

public sealed class CircuitValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not Circuit circuit) return ValidationResult.Success;

        if (circuit is NonSpaceCircuit nonSpaceCircuit)
        {
            if (nonSpaceCircuit.AmpereLoad.HasError)
            {
                return new ValidationResult(nonSpaceCircuit.AmpereLoad.ErrorMessage);
            }
            
            if (nonSpaceCircuit.AmpereTrip.HasError)
            {
                return new ValidationResult(nonSpaceCircuit.AmpereTrip.ErrorMessage);
            }
            
            if (nonSpaceCircuit.AmpereFrame.HasError)
            {
                return new ValidationResult(nonSpaceCircuit.AmpereFrame.ErrorMessage);
            }
            
            if (nonSpaceCircuit.R.HasError && nonSpaceCircuit.R.ErrorType != CalculationErrorType.IsUndefinedSpareCircuitProperty)
            {
                return new ValidationResult(nonSpaceCircuit.R.ErrorMessage);
            }
            
            if (nonSpaceCircuit.X.HasError && nonSpaceCircuit.X.ErrorType != CalculationErrorType.IsUndefinedSpareCircuitProperty)
            {
                return new ValidationResult(nonSpaceCircuit.X.ErrorMessage);
            }
            
            if (nonSpaceCircuit.VoltageDrop.HasError && nonSpaceCircuit.VoltageDrop.ErrorType != CalculationErrorType.IsUndefinedSpareCircuitProperty)
            {
                return new ValidationResult(nonSpaceCircuit.VoltageDrop.ErrorMessage);
            }
            
            if (nonSpaceCircuit.ConductorSize.HasError && nonSpaceCircuit.ConductorSize.ErrorType != CalculationErrorType.IsUndefinedSpareCircuitProperty)
            {
                return new ValidationResult(nonSpaceCircuit.ConductorSize.ErrorMessage);
            }
            
            if (nonSpaceCircuit.GroundingSize.HasError && nonSpaceCircuit.GroundingSize.ErrorType != CalculationErrorType.IsUndefinedSpareCircuitProperty)
            {
                return new ValidationResult(nonSpaceCircuit.GroundingSize.ErrorMessage);
            }
            
            if (nonSpaceCircuit.RacewaySize.HasError && nonSpaceCircuit.RacewaySize.ErrorType != CalculationErrorType.IsUndefinedSpareCircuitProperty)
            {
                return new ValidationResult(nonSpaceCircuit.RacewaySize.ErrorMessage);
            }
        }

        if (circuit is NonSpareCircuit nonSpareCircuit && nonSpareCircuit.VoltAmpere.HasError)
        {
            return new ValidationResult(nonSpareCircuit.VoltAmpere.ErrorMessage);
        }
        
        if (circuit is LightingOutletCircuit { AmpereLoad.Value: > 50 })
        {
            return new ValidationResult("Ampere load cannot exceed 50 for lighting outlet circuits.");
        }

        if (circuit is ConvenienceOutletCircuit { HasExceedingAmpereTrip: true })
        {
            return new ValidationResult("GFCI Receptacle Ampere Trip is limited to 15 AT and 20 AT only.");
        }
        
        return ValidationResult.Success;
    }
}