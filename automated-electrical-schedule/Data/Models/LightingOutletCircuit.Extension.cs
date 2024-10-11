namespace automated_electrical_schedule.Data.Models;

public partial class LightingOutletCircuit
{
    public override double VoltAmpere => Quantity * WattagePerFixture;

    public override double AmpereLoad => Quantity * WattagePerFixture * DemandFactor / 100 / Voltage;

    public override int AmpereTrip => DataUtils.GetAmpereTrip(AmpereLoad / 0.8, 15);

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