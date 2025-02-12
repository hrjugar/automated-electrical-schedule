using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Wrappers;

namespace automated_electrical_schedule.Data.Models;

public partial class ApplianceEquipmentOutletCircuit
{
    public List<ApplianceType> AllowedApplianceTypes
    {
        get
        {
            List<ApplianceType> applianceTypes = [];

            if (!IsItemized)
            {
                applianceTypes.Add(ApplianceType.Dryer);
            }
            
            applianceTypes.AddRange([
                ApplianceType.KitchenEquipment, 
                ApplianceType.Other
            ]);

            return applianceTypes;
        }
    }
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
            CorrectedVoltageDrop = CorrectedVoltageDrop,
            VoltageDropCorrectionConductorSize = VoltageDropCorrectionConductorSize,
            
            ApplianceType = ApplianceType,
            IsItemized = IsItemized,
            Fixtures = Fixtures.Select(fixture => fixture.Clone()).ToList()
        };
    }
}