using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class ConvenienceOutletCircuit
{
    public override double GetVoltAmpere()
    {
        return Quantity * 180;
    }

    public override double GetAmpereLoad()
    {
        if (OutletType == OutletType.FourGang) return 4 * 360 * DemandFactor / GetVoltage();

        return (int)OutletType * 180 * DemandFactor / GetVoltage();
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