using System.ComponentModel.DataAnnotations;

namespace automated_electrical_schedule.Data.Validators;

public class ProjectNameValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var name = (string?) value;
        if (string.IsNullOrEmpty(name)) return ValidationResult.Success;

        using var context = new DatabaseContext();
        if (context is null)
        {
            throw new InvalidOperationException("DatabaseContext is not available.");
        }
        
        return context.Projects.Any(existingProject => existingProject.ProjectName == name)
            ? new ValidationResult("Project name already exists.", new[] { validationContext.MemberName })
            : ValidationResult.Success;
    }
}