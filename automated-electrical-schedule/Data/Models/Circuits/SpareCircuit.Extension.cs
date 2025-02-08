using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Wrappers;

namespace automated_electrical_schedule.Data.Models;

public partial class SpareCircuit
{
    public override CalculationResult<double> AmpereLoad =>
        CalculationResult<double>.Success(VoltAmpere / Voltage);

    public override CalculationResult<int> AmpereTrip =>
        DataUtils.GetAmpereTrip(CalculationResult<double>.Success(AmpereLoad.Value / 0.8), 15);
    
    public override CalculationResult<double> ConductorSize =>
        ConductorType is null || WireLength == 0
            ? CalculationResult<double>.Failure(CalculationErrorType.IsNullableSpareCircuitProperty)
            : base.ConductorSize;

    public override string ConductorTextDisplay =>
        ConductorSize.ErrorType == CalculationErrorType.IsNullableSpareCircuitProperty
            ? "-"
            : base.ConductorTextDisplay;

    public override CalculationResult<double> GroundingSize =>
        Grounding is null || WireLength == 0
            ? CalculationResult<double>.Failure(CalculationErrorType.IsNullableSpareCircuitProperty)
            : base.GroundingSize;

    public override string GroundingTextDisplay =>
        GroundingSize.ErrorType == CalculationErrorType.IsNullableSpareCircuitProperty
            ? "-"
            : base.GroundingTextDisplay;

    public override CalculationResult<double?> R =>
        ConductorType is null
            ? CalculationResult<double?>.Failure(CalculationErrorType.IsNullableSpareCircuitProperty)
            : base.R;
    
    public override CalculationResult<double?> X =>
        ConductorType is null
            ? CalculationResult<double?>.Failure(CalculationErrorType.IsNullableSpareCircuitProperty)
            : base.X; 

    public override CalculationResult<double?> VoltageDrop =>
        ConductorType is null
            ? CalculationResult<double?>.Failure(CalculationErrorType.IsNullableSpareCircuitProperty)
            : base.VoltageDrop; 

    public override CalculationResult<int> RacewaySize =>
        ConductorType is null || Grounding is null || RacewayType == RacewayType.None || WireLength == 0
            ? CalculationResult<int>.Failure(CalculationErrorType.IsNullableSpareCircuitProperty)
            : base.RacewaySize;

    public override string RacewayTextDisplay =>
        RacewaySize.ErrorType == CalculationErrorType.IsNullableSpareCircuitProperty
            ? "-"
            : base.RacewayTextDisplay;

    public override Circuit Clone()
    {
        return new SpareCircuit
        {
            Id = Id,
            ParentDistributionBoardId = ParentDistributionBoardId,
            ParentDistributionBoard = ParentDistributionBoard,
            Order = Order,
            CircuitType = CircuitType,
            LineToLineVoltage = LineToLineVoltage,
            VoltAmpere = VoltAmpere,
            WireLength = WireLength,
            SetCount = SetCount,
            ConductorTypeId = ConductorTypeId,
            GroundingId = GroundingId,
            RacewayType = RacewayType
        };
    }
}