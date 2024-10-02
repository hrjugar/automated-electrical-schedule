using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class Circuit
{
    public abstract Circuit Clone();

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
    public abstract int GetAmpereTrip();

    public int GetAmpereFrame()
    {
        return DataConstants.GetAmpereFrame(GetAmpereTrip());
    }

    public double GetVoltageDrop()
    {
        // TODO: Create this
        return 1;
    }

    public virtual double GetConductorSize()
    {
        return ConductorSizingTable.GetConductorSize(ConductorType, GetAmpereTrip());
    }

    public double GetGroundingSize()
    {
        return CircuitGroundingSizingTable.GetGroundingSize(Grounding.Material, GetAmpereTrip());
    }
}