using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;

namespace automated_electrical_schedule.Data.Models;

public partial class ConvenienceOutletCircuit
{
    public override double VoltAmpere => Quantity * 180;

    public override double AmpereLoad
    {
        get
        {
            if (OutletType == OutletType.FourGang) return 4 * 360 * DemandFactor / 100 / Voltage;

            return 180 * Quantity * (DemandFactor / 100) / Voltage;
        }
    }

    public override int AmpereTrip => DataUtils.GetAmpereTrip(AmpereLoad / 0.8, 20);

    public override double ConductorSize => ConductorSizeTable.GetConductorSize(ConductorType, AmpereTrip, 3.5);

    public override Circuit Clone()
    {
        return new ConvenienceOutletCircuit
        {
            Id = Id,
            ParentDistributionBoardId = ParentDistributionBoardId,
            ParentDistributionBoard = ParentDistributionBoard,
            CircuitType = CircuitType,
            LineToLineVoltage = LineToLineVoltage,
            Description = Description,
            Quantity = Quantity,
            WireLength = WireLength,
            DemandFactor = DemandFactor,
            CircuitProtection = CircuitProtection,
            SetCount = SetCount,
            ConductorTypeId = ConductorTypeId,
            GroundingId = GroundingId,
            RacewayType = RacewayType,

            OutletType = OutletType
        };
    }
}