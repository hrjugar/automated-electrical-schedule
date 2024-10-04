using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class DistributionBoard
{
    public abstract DistributionBoard Clone();

    public abstract List<BoardVoltage> GetAllowedVoltages();

    public double GetTotalVoltAmpere()
    {
        return Circuits.Sum(circuit => circuit.GetVoltAmpere());
    }

    public double GetTotalAmpereLoad(LineToLineVoltage? lineToLineVoltage = null)
    {
        return lineToLineVoltage == null
            ? Circuits
                .Sum(circuit => circuit.GetAmpereLoad())
            : Circuits
                .Where(circuit => circuit.LineToLineVoltage == lineToLineVoltage)
                .Sum(circuit => circuit.GetAmpereLoad());
    }
}