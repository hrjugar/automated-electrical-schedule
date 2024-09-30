namespace automated_electrical_schedule.Data.Models;

public partial class ApplianceEquipmentOutletCircuit
{
    public override double GetVoltAmpere()
    {
        return Wattage;
    }

    public override double GetAmpereLoad()
    {
        return GetVoltAmpere() * DemandFactor / GetVoltage();
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