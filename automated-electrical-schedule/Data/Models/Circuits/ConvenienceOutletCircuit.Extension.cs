using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;
using automated_electrical_schedule.Data.Wrappers;

namespace automated_electrical_schedule.Data.Models;

public partial class ConvenienceOutletCircuit
{
    public override List<CircuitProtection> AllowedCircuitProtections =>
    [
        CircuitProtection.MiniatureCircuitBreaker,
        CircuitProtection.MoldedCaseCircuitBreaker,
        CircuitProtection.GfciCircuitBreaker
    ];

    public bool HasExceedingAmpereTrip =>
        CircuitProtection != CircuitProtection.GfciCircuitBreaker &&
        GfciReceptacleQuantity > 0 &&
        AmpereTrip.Value > 20;

    public override CalculationResult<double> VoltAmpere =>
        CalculationResult<double>.Success(
            GfciReceptacleQuantity * GfciReceptacleYoke +
            OneGangQuantity * OneGangYoke +
            TwoGangQuantity * TwoGangYoke +
            ThreeGangQuantity * ThreeGangYoke +
            FourGangQuantity * FourGangYoke
        );

    public override CalculationResult<double> AmpereLoad => 
        CalculationResult<double>.Success(VoltAmpere.Value / Voltage);

    public override CalculationResult<int> AmpereTrip => DataUtils.GetAmpereTrip(
        CalculationResult<double>.Success(AmpereLoad.Value / 0.8), 
        20
    );

    public override CalculationResult<double> ConductorSize => ConductorSizeTable.GetConductorSize(ConductorType, AmpereTrip, SetCount, 3.5);

    public void ResolveExceedingAmpereTrip()
    {
        if (!HasExceedingAmpereTrip) return;
        
        CircuitProtection = CircuitProtection.GfciCircuitBreaker;
        TwoGangQuantity += GfciReceptacleQuantity;
        GfciReceptacleQuantity = 0;
        GfciReceptacleYoke = 0;
    }
    
    public override Circuit Clone()
    {
        return new ConvenienceOutletCircuit
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
            
            GfciReceptacleQuantity = GfciReceptacleQuantity,
            GfciReceptacleYoke = GfciReceptacleYoke,
            OneGangQuantity = OneGangQuantity,
            OneGangYoke = OneGangYoke,
            TwoGangQuantity = TwoGangQuantity,
            TwoGangYoke = TwoGangYoke,
            ThreeGangQuantity = ThreeGangQuantity,
            ThreeGangYoke = ThreeGangYoke,
            FourGangQuantity = FourGangQuantity,
            FourGangYoke = FourGangYoke
        };
    }
}