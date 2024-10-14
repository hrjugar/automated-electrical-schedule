using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;

namespace automated_electrical_schedule.Data.Models;

public partial class ConvenienceOutletCircuit
{
    public override CalculationResult<double> VoltAmpere => OutletType == OutletType.FourGang
        ? CalculationResult<double>.Success(Quantity * 360)
        : CalculationResult<double>.Success(Quantity * 180);

    public override CalculationResult<double> AmpereLoad => CalculationResult<double>.Success(
        OutletType == OutletType.FourGang
            ? 4 * 360 * DemandFactor / 100 / Voltage
            : 180 * Quantity * (DemandFactor / 100) / Voltage
    );

    public override CalculationResult<int> AmpereTrip => DataUtils.GetAmpereTrip(
        CalculationResult<double>.Success(AmpereLoad.Value / 0.8), 
        20
    );

    public override CalculationResult<double> ConductorSize => ConductorSizeTable.GetConductorSize(ConductorType, AmpereTrip, 3.5);

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