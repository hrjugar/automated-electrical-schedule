using System.ComponentModel.DataAnnotations;
using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;

namespace automated_electrical_schedule.Data.Validators;

public class HorsepowerValidator : ValidationAttribute
{
    public string IdProperty { get; set; }
    public string ParentDistributionBoardProperty { get; set; }
    public string MotorApplicationProperty { get; set; }
    public string HvacGroupCodeProperty { get; set; }

    public HorsepowerValidator(
        string idProperty,
        string parentDistributionBoardProperty,
        string motorApplicationProperty,
        string hvacGroupCodeProperty
    )
    {
        if (string.IsNullOrEmpty(idProperty))
        {
            throw new ArgumentNullException(nameof(idProperty));
        }
        
        if (string.IsNullOrEmpty(parentDistributionBoardProperty))
        {
            throw new ArgumentNullException(nameof(parentDistributionBoardProperty));
        }
        
        if (string.IsNullOrEmpty(motorApplicationProperty))
        {
            throw new ArgumentNullException(nameof(motorApplicationProperty));
        }
        
        if (string.IsNullOrEmpty(hvacGroupCodeProperty))
        {
            throw new ArgumentNullException(nameof(hvacGroupCodeProperty));
        }

        IdProperty = idProperty;
        ParentDistributionBoardProperty = parentDistributionBoardProperty;
        MotorApplicationProperty = motorApplicationProperty;
        HvacGroupCodeProperty = hvacGroupCodeProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || (string) value == string.Empty) return ValidationResult.Success;

        var idProperty = 
            validationContext.ObjectInstance.GetType().GetProperty(IdProperty);
        var idValue = 
            (int) idProperty.GetValue(validationContext.ObjectInstance, null);
        
        var parentDistributionBoardProperty = 
            validationContext.ObjectInstance.GetType().GetProperty(ParentDistributionBoardProperty);
        var parentDistributionBoardValue = 
            (DistributionBoard) parentDistributionBoardProperty.GetValue(validationContext.ObjectInstance, null);

        var motorApplicationProperty =
            validationContext.ObjectInstance.GetType().GetProperty(MotorApplicationProperty);
        var motorApplicationValue =
            (MotorApplication)motorApplicationProperty.GetValue(validationContext.ObjectInstance, null);
            
        var hvacGroupCodeProperty =
            validationContext.ObjectInstance.GetType().GetProperty(HvacGroupCodeProperty);
        var hvacGroupCodeValue =
            (string?) hvacGroupCodeProperty.GetValue(validationContext.ObjectInstance, null);
        
        if (
            motorApplicationValue != MotorApplication.GroupedHvac || 
            string.IsNullOrEmpty(hvacGroupCodeValue)
        ) return ValidationResult.Success;

        var hvacGroupHorsepowers = parentDistributionBoardValue
            .FilterNestedCircuits<MotorOutletCircuit>(mc =>
                mc.MotorApplication == MotorApplication.GroupedHvac &&
                mc.HvacGroupCode == hvacGroupCodeValue &&
                mc.Id != idValue
            )
            .Select(mc => mc.Horsepower)
            .ToList();

        if (hvacGroupHorsepowers.Count > 0 && hvacGroupHorsepowers[0] != (string)value)
        {
            return new ValidationResult(
                $"Horsepower should be equal to {hvacGroupHorsepowers[0]} in heating/air conditioning group {hvacGroupCodeValue}",
                new[] { validationContext.MemberName }
            );
        }
        
        return ValidationResult.Success;
    }
}