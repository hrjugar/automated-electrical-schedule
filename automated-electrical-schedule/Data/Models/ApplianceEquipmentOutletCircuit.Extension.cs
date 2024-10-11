namespace automated_electrical_schedule.Data.Models;

public partial class ApplianceEquipmentOutletCircuit
{
    public override double VoltAmpere => Wattage;

    public override double AmpereLoad => VoltAmpere * (DemandFactor / 100) / Voltage;

    public override int AmpereTrip => DataUtils.GetAmpereTrip(AmpereLoad / 0.8, 20);

    public override Circuit Clone()
    {
        return new ApplianceEquipmentOutletCircuit
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

            Wattage = Wattage
        };
    }
}