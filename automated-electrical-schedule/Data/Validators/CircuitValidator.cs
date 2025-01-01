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

        // if (circuit is ConvenienceOutletCircuit convenienceOutletCircuit)
        // {
        //     if (convenienceOutletCircuit.GfciReceptacleQuantity > 0 &&
        //         convenienceOutletCircuit.GfciReceptacleYoke < 180)
        //     {
        //         return new ValidationResult("If GFCI receptacle exists, its VA must be at least 180.");
        //     }
        //     
        //     if (convenienceOutletCircuit.OneGangQuantity > 0 &&
        //              convenienceOutletCircuit.OneGangYoke < 180)
        //     {
        //         return new ValidationResult("If one-gang receptacle exists, its VA must be at least 180.");
        //     }
        //     
        //     if (convenienceOutletCircuit.TwoGangQuantity > 0 &&
        //              convenienceOutletCircuit.TwoGangYoke < 180)
        //     {
        //         return new ValidationResult("If two-gang receptacle exists, its VA must be at least 180.");
        //     }
        //     
        //     if (convenienceOutletCircuit.ThreeGangQuantity > 0 &&
        //              convenienceOutletCircuit.ThreeGangYoke < 180)
        //     {
        //         return new ValidationResult("If three-gang receptacle exists, its VA must be at least 180.");
        //     }
        //     
        //     if (convenienceOutletCircuit.FourGangQuantity > 0 &&
        //              convenienceOutletCircuit.FourGangYoke < 360)
        //     {
        //         return new ValidationResult("If four-gang receptacle exists, its VA must be at least 360.");
        //     }
        // }
        
        return ValidationResult.Success;
    }
}