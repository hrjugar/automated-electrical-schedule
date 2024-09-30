using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class Circuit
{
    public virtual List<CircuitProtection> GetAllowedCircuitProtections()
    {
        return
        [
            CircuitProtection.MiniatureCircuitBreaker,
            CircuitProtection.MoldedCaseCircuitBreaker
        ];
    }

    public int GetVoltage()
    {
        return (int)ParentDistributionBoard.Voltage;
    }

    public abstract double GetVoltAmpere();

    public abstract double GetAmpereLoad();
    public abstract double GetAmpereTrip();
    public abstract double GetAmpereFrame();

    public double GetVoltageDrop()
    {
        // TODO: Create this
        return 1;
    }
}