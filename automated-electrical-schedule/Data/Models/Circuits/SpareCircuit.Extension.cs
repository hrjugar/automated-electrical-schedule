using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Wrappers;

namespace automated_electrical_schedule.Data.Models;

public partial class SpareCircuit
{
    public override CalculationResult<double> AmpereLoad
    {
        get
        {
            var factor = LineToLineVoltage == LineToLineVoltage.Abc ? Math.Sqrt(3) : 1;
            return CalculationResult<double>.Success(VoltAmpere / (factor * Voltage));
        }
    }

    public override CalculationResult<int> AmpereTrip =>
        DataUtils.GetAmpereTrip(CalculationResult<double>.Success(AmpereLoad.Value / 0.8), 15);
    
    public override CalculationResult<double> ConductorSize =>
        ConductorType is null
            ? CalculationResult<double>.Failure(CalculationErrorType.IsUndefinedSpareCircuitProperty)
            : base.ConductorSize;

    public override string ConductorTextDisplay =>
        ConductorSize.ErrorType == CalculationErrorType.IsUndefinedSpareCircuitProperty
            ? "-"
            : base.ConductorTextDisplay;

    public override CalculationResult<double> GroundingSize =>
        Grounding is null
            ? CalculationResult<double>.Failure(CalculationErrorType.IsUndefinedSpareCircuitProperty)
            : base.GroundingSize;

    public override string GroundingTextDisplay =>
        GroundingSize.ErrorType == CalculationErrorType.IsUndefinedSpareCircuitProperty
            ? "-"
            : base.GroundingTextDisplay;

    public override CalculationResult<double?> R =>
        ConductorType is null
            ? CalculationResult<double?>.Failure(CalculationErrorType.IsUndefinedSpareCircuitProperty)
            : base.R;
    
    public override CalculationResult<double?> X =>
        ConductorType is null
            ? CalculationResult<double?>.Failure(CalculationErrorType.IsUndefinedSpareCircuitProperty)
            : base.X; 

    public override CalculationResult<double?> VoltageDrop =>
        ConductorType is null
            ? CalculationResult<double?>.Failure(CalculationErrorType.IsUndefinedSpareCircuitProperty)
            : base.VoltageDrop;

    public override bool CanCorrectVoltageDropWithConductorSize =>
        ConductorType is not null && base.CanCorrectVoltageDropWithConductorSize;

    public override CalculationResult<int> RacewaySize =>
        ConductorType is null || Grounding is null || RacewayType == RacewayType.None
            ? CalculationResult<int>.Failure(CalculationErrorType.IsUndefinedSpareCircuitProperty)
            : base.RacewaySize;

    public override string RacewayTextDisplay =>
        RacewaySize.ErrorType == CalculationErrorType.IsUndefinedSpareCircuitProperty
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
            RacewayType = RacewayType,
            CorrectedVoltageDrop = CorrectedVoltageDrop,
            VoltageDropCorrectionConductorSize = VoltageDropCorrectionConductorSize
        };
    }
}