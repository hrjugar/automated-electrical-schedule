namespace automated_electrical_schedule.Data.Models;

public partial class LightingOutletCircuit
{
    public override double GetVoltAmpere()
    {
        return Quantity * WattagePerFixture;
    }

    public override double GetAmpereLoad()
    {
        return Quantity * WattagePerFixture * DemandFactor / GetVoltage();
    }

    public override double GetAmpereTrip()
    {
        // TODO: Update formula
        return 1;
    }

    public override double GetAmpereFrame()
    {
        // TODO: Update formula
        return 1;
    }
}