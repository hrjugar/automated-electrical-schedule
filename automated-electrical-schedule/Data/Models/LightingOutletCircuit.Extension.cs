namespace automated_electrical_schedule.Data.Models;

public partial class LightingOutletCircuit
{
    public override CalculationResult<double> VoltAmpere => CalculationResult<double>.Success(Quantity * WattagePerFixture);

    public override CalculationResult<double> AmpereLoad => 
        CalculationResult<double>.Success(Quantity * WattagePerFixture * DemandFactor / 100 / Voltage);

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

            WattagePerFixture = WattagePerFixture
        };
    }
}