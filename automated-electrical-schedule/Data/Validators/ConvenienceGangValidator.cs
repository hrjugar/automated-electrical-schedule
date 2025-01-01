using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Validators;

public class ConvenienceGangValidator : ValidationAttribute
{
    public string QuantityProperty { get; set; }
    public double MinValue { get; set; }

    public ConvenienceGangValidator(string quantityProperty, double minValue)
    {
        if (string.IsNullOrEmpty(quantityProperty))
        {
            throw new ArgumentNullException(nameof(quantityProperty));
        }
        
        QuantityProperty = quantityProperty;
        MinValue = minValue;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            var quantityProperty = validationContext.ObjectInstance.GetType().GetProperty(QuantityProperty);
            var quantityValue = (int) quantityProperty.GetValue(validationContext.ObjectInstance, null);

            if (quantityValue > 0 && (double)value < MinValue)
            {
                return new ValidationResult(
                    $"{validationContext.DisplayName} should not be lower than {MinValue}.", 
                    new[] { validationContext.MemberName });
            }
        }
        
        return ValidationResult.Success;
    }
}