using System.ComponentModel.DataAnnotations;
using automated_electrical_schedule.Data.Models;

namespace automated_electrical_schedule.Data.Validators;

public sealed class CircuitValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not Circuit circuit) return ValidationResult.Success;
        
        if (circuit.VoltAmpere.HasError)
        {
            return new ValidationResult(circuit.VoltAmpere.ErrorMessage);
        }

        if (circuit.AmpereLoad.HasError)
        {
            return new ValidationResult(circuit.AmpereLoad.ErrorMessage);
        }
        
        if (circuit.AmpereTrip.HasError)
        {
            return new ValidationResult(circuit.AmpereTrip.ErrorMessage);
        }
        
        if (circuit.AmpereFrame.HasError)
        {
            return new ValidationResult(circuit.AmpereFrame.ErrorMessage);
        }

        if (circuit.R.HasError)
        {
            return new ValidationResult(circuit.R.ErrorMessage);
        }
        
        if (circuit.X.HasError)
        {
            return new ValidationResult(circuit.X.ErrorMessage);
        }

        if (circuit.VoltageDrop.HasError)
        {
            return new ValidationResult(circuit.VoltageDrop.ErrorMessage);
        }

        if (circuit.ConductorSize.HasError)
        {
            return new ValidationResult(circuit.ConductorSize.ErrorMessage);
        }
        
        if (circuit.GroundingSize.HasError)
        {
            return new ValidationResult(circuit.GroundingSize.ErrorMessage);
        }
        
        if (circuit.RacewaySize.HasError)
        {
            return new ValidationResult(circuit.RacewaySize.ErrorMessage);
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