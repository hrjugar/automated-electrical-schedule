using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;

namespace automated_electrical_schedule.Extensions;

public static class CalculationErrorExtensions
{
    public static int Sum(this IEnumerable<CalculationResult<int>> source)
    {
        var successResults = source.Where(x => !x.HasError).ToList();
        
        return successResults.Count == 0
            ? 0
            : successResults.Select(successResult => successResult.Value).Sum();
    }
    
    public static double Sum(this IEnumerable<CalculationResult<double>> source)
    {
        var successResults = source.Where(x => !x.HasError).ToList();
        
        return successResults.Count == 0
            ? 0
            : successResults.Select(successResult => successResult.Value).Sum();
    }
    
    public static int Max(this IEnumerable<CalculationResult<int>> source)
    {
        var successResults = source.Where(x => !x.HasError).ToList();
        
        return successResults.Count == 0
            ? 0
            : successResults.Select(successResult => successResult.Value).Max();
    }
    
    public static double Max(this IEnumerable<CalculationResult<double>> source)
    {
        var successResults = source.Where(x => !x.HasError).ToList();

        return successResults.Count == 0
            ? 0
            : successResults.Select(successResult => successResult.Value).Max();
    }
}