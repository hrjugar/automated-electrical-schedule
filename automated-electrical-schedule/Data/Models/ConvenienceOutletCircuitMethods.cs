using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;

namespace automated_electrical_schedule.Data.Models;

public partial class ConvenienceOutletCircuit
{
    public override Circuit Clone()
    {
        return new ConvenienceOutletCircuit
        {
            Id = Id,
            ParentDistributionBoardId = ParentDistributionBoardId,
            ParentDistributionBoard = ParentDistributionBoard,
            CircuitType = CircuitType,
            Description = Description,
            Quantity = Quantity,
            WireLength = WireLength,
            DemandFactor = DemandFactor,
            CircuitProtection = CircuitProtection,
            SetCount = SetCount,
            ConductorTypeId = ConductorTypeId,
            ConductorType = ConductorType,
            GroundingId = GroundingId,
            Grounding = Grounding,
            RacewayType = RacewayType,

            OutletType = OutletType
        };
    }

    public override double GetVoltAmpere()
    {
        return Quantity * 180;
    }

    public override double GetAmpereLoad()
    {
        if (OutletType == OutletType.FourGang) return 4 * 360 * DemandFactor / 100 / GetVoltage();

        return 180 * Quantity * (DemandFactor / 100) / GetVoltage();
    }

    public override int GetAmpereTrip()
    {
        return DataUtils.GetAmpereTrip(GetAmpereLoad() / 0.8, 20);
    }

    public override double GetConductorSize()
    {
        return ConductorSizeTable.GetConductorSize(ConductorType, GetAmpereTrip(), 3.5);
    }
}