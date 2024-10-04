using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;

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
        return DataUtils.GetAmpereFrame(GetAmpereTrip());
    }

    public double GetR()
    {
        return VoltageDropTable.GetR(
            RacewayType,
            ConductorType.Material,
            GetConductorSize()
        );
    }

    public double GetX()
    {
        return VoltageDropTable.GetX(
            RacewayType,
            GetConductorSize()
        );
    }

    public double GetVoltageDrop()
    {
        return VoltageDropTable.GetVoltageDrop(
            LineToLineVoltage,
            GetR(),
            GetX(),
            GetAmpereLoad(),
            WireLength,
            SetCount,
            GetVoltage()
        );
    }

    public virtual double GetConductorSize()
    {
        return ConductorSizeTable.GetConductorSize(ConductorType, GetAmpereTrip());
    }

    public double GetGroundingSize()
    {
        return CircuitGroundingSizeTable.GetGroundingSize(Grounding.Material, GetAmpereTrip());
    }

    public int GetRacewaySize()
    {
        var wireCount = LineToLineVoltage == Enums.LineToLineVoltage.Abc ? 4 : 3;
        return RacewaySizeTable.GetRacewaySize(
            ConductorType.WireType,
            RacewayType,
            GetConductorSize(),
            wireCount
        );
    }

    public void CorrectVoltageDrop()
    {
        while (GetVoltageDrop() * 100 >= 3) SetCount += 1;
    }
}