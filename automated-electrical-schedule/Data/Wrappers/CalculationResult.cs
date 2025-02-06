using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.Wrappers;

public struct CalculationResult<T>
{
    public T Value { get; }
    public CalculationErrorType ErrorType { get; }
    public bool HasError => ErrorType != CalculationErrorType.NoError;
    
    private CalculationResult(T value)
    {
        Value = value;
        ErrorType = CalculationErrorType.NoError;
    }
    
    private CalculationResult(CalculationErrorType errorType)
    {
        Value = default!;
        ErrorType = errorType;
    }
    
    public static CalculationResult<T> Success(T value) => new(value);
    public static CalculationResult<T> Failure(CalculationErrorType errorType) => new(errorType);

    public override string ToString()
    {
        return HasError
            ? ErrorType.GetDisplayName()
            : Value?.ToString() ?? "X";
    }

    // public string ErrorString()
    // {
    //     return ErrorType.GetDisplayName();
    // }

    public string ErrorMessage => ErrorType.GetDisplayName();
}