using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Extensions;

public static class LineToLineVoltageExtensions
{
    public static string GetDisplayName(this LineToLineVoltage lineToLineVoltage, ThreePhaseConfiguration? config = null)
    {
        return config switch
        {
            ThreePhaseConfiguration.Delta => lineToLineVoltage switch
            {
                LineToLineVoltage.A => "AB",
                LineToLineVoltage.B => "BC",
                LineToLineVoltage.C => "CA",
                LineToLineVoltage.Abc => "ABC",
                LineToLineVoltage.None => "",
                _ => throw new ArgumentOutOfRangeException(nameof(lineToLineVoltage), lineToLineVoltage, null)
            },
            ThreePhaseConfiguration.Wye => lineToLineVoltage switch
            {
                LineToLineVoltage.A => "AN",
                LineToLineVoltage.B => "BN",
                LineToLineVoltage.C => "CN",
                LineToLineVoltage.Abc => "ABC",
                LineToLineVoltage.None => "",
                _ => throw new ArgumentOutOfRangeException(nameof(lineToLineVoltage), lineToLineVoltage, null)
            },
            _ => "-"
        };
    }
}