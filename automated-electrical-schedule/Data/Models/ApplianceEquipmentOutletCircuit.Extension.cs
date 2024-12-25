namespace automated_electrical_schedule.Data.Models;

public partial class ApplianceEquipmentOutletCircuit
{
    public override CalculationResult<double> VoltAmpere => CalculationResult<double>.Success(Wattage);

    public override CalculationResult<double> AmpereLoad => CalculationResult<double>.Success(VoltAmpere.Value / Voltage);

    public override CalculationResult<int> AmpereTrip => DataUtils.GetAmpereTrip(
        CalculationResult<double>.Success(AmpereLoad.Value / 0.8), 
        20);

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
            CircuitProtection = CircuitProtection,
            SetCount = SetCount,
            ConductorTypeId = ConductorTypeId,
            GroundingId = GroundingId,
            RacewayType = RacewayType,

            Wattage = Wattage
        };
    }
}