using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;

namespace automated_electrical_schedule.Data.Models;

public partial class ConvenienceOutletCircuit
{
    // TODO: Change this later
    public override CalculationResult<double> VoltAmpere => OutletType == OutletType.FourGang
        ? CalculationResult<double>.Success(360)
        : CalculationResult<double>.Success(180);

    // TODO: Change this later
    public override CalculationResult<double> AmpereLoad => CalculationResult<double>.Success(
        OutletType == OutletType.FourGang
            ? 4.0 * 360 / Voltage
            : 180.0 / Voltage
    );

    public override CalculationResult<int> AmpereTrip => DataUtils.GetAmpereTrip(
        CalculationResult<double>.Success(AmpereLoad.Value / 0.8), 
        20
    );

    public override CalculationResult<double> ConductorSize => ConductorSizeTable.GetConductorSize(ConductorType, AmpereTrip, SetCount, 3.5);

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
            WireLength = WireLength,
            CircuitProtection = CircuitProtection,
            SetCount = SetCount,
            ConductorTypeId = ConductorTypeId,
            GroundingId = GroundingId,
            RacewayType = RacewayType,

            OutletType = OutletType
        };
    }
}