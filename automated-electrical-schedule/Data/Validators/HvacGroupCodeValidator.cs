using System.ComponentModel.DataAnnotations;
using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Validators;

public class HvacGroupCodeValidator : ValidationAttribute
{
    public string MotorApplicationProperty { get; set; }

    public HvacGroupCodeValidator(string motorApplicationProperty)
    {
        if (string.IsNullOrEmpty(motorApplicationProperty))
        {
            throw new ArgumentNullException(nameof(motorApplicationProperty));
        }

        MotorApplicationProperty = motorApplicationProperty;
    }
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var motorApplicationProperty =
            validationContext.ObjectInstance.GetType().GetProperty(MotorApplicationProperty);
        var motorApplicationValue =
            (MotorApplication)motorApplicationProperty.GetValue(validationContext.ObjectInstance, null);
        
        if (motorApplicationValue != MotorApplication.GroupedHvac) return ValidationResult.Success;

        if (value is null || (string)value == string.Empty)
        {
            return new ValidationResult($"HVAC group code should not be empty.",
                new[] { validationContext.MemberName });
        }
        
        return ValidationResult.Success;
    }
}