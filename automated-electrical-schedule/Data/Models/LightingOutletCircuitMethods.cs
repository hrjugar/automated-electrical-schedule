namespace automated_electrical_schedule.Data.Models;

public partial class LightingOutletCircuit
{
    public override Circuit Clone()
    {
        return new LightingOutletCircuit
        {
            Id = Id,
            ParentDistributionBoardId = ParentDistributionBoardId,
            ParentDistributionBoard = ParentDistributionBoard,
            CircuitType = CircuitType,
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

            Description = Description,
            WattagePerFixture = WattagePerFixture
        };
    }


    public override double GetVoltAmpere()
    {
        return Quantity * WattagePerFixture;
    }

    public override double GetAmpereLoad()
    {
        return Quantity * WattagePerFixture * DemandFactor / 100 / GetVoltage();
    }

    public override int GetAmpereTrip()
    {
        return DataConstants.GetAmpereTrip(GetAmpereLoad() / 0.8, 15);
    }
}