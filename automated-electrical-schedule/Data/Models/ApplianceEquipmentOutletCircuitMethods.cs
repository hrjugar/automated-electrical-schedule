namespace automated_electrical_schedule.Data.Models;

public partial class ApplianceEquipmentOutletCircuit
{
    public override Circuit Clone()
    {
        return new ApplianceEquipmentOutletCircuit
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

            Wattage = Wattage
        };
    }

    public override double GetVoltAmpere()
    {
        return Wattage;
    }

    public override double GetAmpereLoad()
    {
        return GetVoltAmpere() * (DemandFactor / 100) / GetVoltage();
    }

    public override int GetAmpereTrip()
    {
        return DataConstants.GetAmpereTrip(GetAmpereLoad() / 0.8, 20);
    }
}