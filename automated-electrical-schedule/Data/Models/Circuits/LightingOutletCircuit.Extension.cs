using automated_electrical_schedule.Data.Wrappers;

namespace automated_electrical_schedule.Data.Models;

public partial class LightingOutletCircuit
{
    public override CalculationResult<int> AmpereTrip => 
        DataUtils.GetAmpereTrip(
            CalculationResult<double>.Success(AmpereLoad.Value / 0.8), 15);

    public override Circuit Clone()
    {
        return new LightingOutletCircuit
        {
            Id = Id,
            ParentDistributionBoardId = ParentDistributionBoardId,
            ParentDistributionBoard = ParentDistributionBoard,
            Order = Order,
            CircuitType = CircuitType,
            LineToLineVoltage = LineToLineVoltage,
            Description = Description,
            WireLength = WireLength,
            CircuitProtection = CircuitProtection,
            SetCount = SetCount,
            ConductorTypeId = ConductorTypeId,
            GroundingId = GroundingId,
            RacewayType = RacewayType,
            VoltageDropCorrectionConductorSize = VoltageDropCorrectionConductorSize,
            
            IsItemized = IsItemized,
            Fixtures = Fixtures.Select(fixture => fixture.Clone()).ToList()
        };
    }
}