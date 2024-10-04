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

    public List<LineToLineVoltage> GetAllowedLineToLineVoltages()
    {
        if (ParentDistributionBoard is ThreePhaseDistributionBoard threePhaseDistributionBoard)
            return threePhaseDistributionBoard.LineToLineVoltage switch
            {
                Enums.LineToLineVoltage.Ab => [Enums.LineToLineVoltage.Ab],
                Enums.LineToLineVoltage.Bc => [Enums.LineToLineVoltage.Bc],
                Enums.LineToLineVoltage.Ca => [Enums.LineToLineVoltage.Ca],
                Enums.LineToLineVoltage.Abc => this switch
                {
                    ConvenienceOutletCircuit or LightingOutletCircuit =>
                    [
                        Enums.LineToLineVoltage.Ab,
                        Enums.LineToLineVoltage.Bc,
                        Enums.LineToLineVoltage.Ca
                    ],
                    ApplianceEquipmentOutletCircuit or MotorOutletCircuit =>
                    [
                        Enums.LineToLineVoltage.Ab,
                        Enums.LineToLineVoltage.Bc,
                        Enums.LineToLineVoltage.Ca,
                        Enums.LineToLineVoltage.Abc
                    ],
                    _ => throw new ArgumentOutOfRangeException(nameof(LineToLineVoltage))
                },
                _ => throw new ArgumentOutOfRangeException(nameof(LineToLineVoltage))
            };

        return [];
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

    public int GetConductorWireCount()
    {
        return LineToLineVoltage == Enums.LineToLineVoltage.Abc ? 3 : 2;
    }

    public int GetRacewaySize()
    {
        var wireCount = GetConductorWireCount() + 1;
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