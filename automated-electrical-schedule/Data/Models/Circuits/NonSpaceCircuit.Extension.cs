using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;
using automated_electrical_schedule.Data.Records;
using automated_electrical_schedule.Data.Wrappers;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class NonSpaceCircuit
{
    public abstract CalculationResult<double> AmpereLoad { get; }
    public abstract CalculationResult<int> AmpereTrip { get; }

    public static List<RacewayType> GetAllowedRacewayTypesStatic(CircuitType circuitType)
    {
        var racewayTypes = Enum.GetValues<RacewayType>().ToList();
        return circuitType == CircuitType.SpareOutlet
            ? racewayTypes
            : racewayTypes.Where(r => r != RacewayType.None).ToList();;
    }
    
    public virtual List<CircuitProtection> AllowedCircuitProtections =>
    [
        CircuitProtection.MiniatureCircuitBreaker,
        CircuitProtection.MoldedCaseCircuitBreaker
    ];

    public List<RacewayType> AllowedRacewayTypes => GetAllowedRacewayTypesStatic(CircuitType);
    
    public int Voltage
    {
        get
        {
            if (
                ParentDistributionBoard is ThreePhaseDistributionBoard parentThreePhaseBoard &&
                parentThreePhaseBoard.ThreePhaseConfiguration == ThreePhaseConfiguration.Wye &&
                (
                    LineToLineVoltage == LineToLineVoltage.A ||
                    LineToLineVoltage == LineToLineVoltage.B ||
                    LineToLineVoltage == LineToLineVoltage.C
                )
            )
                return (int)BoardVoltage.V230;
    
            return (int)ParentDistributionBoard.Voltage;
        }
    }
    
    public int Phase => LineToLineVoltage == LineToLineVoltage.Abc ? 3 : 1;
    
    public int Pole
    {
        get
        {
            if (LineToLineVoltage == LineToLineVoltage.Abc) return 3;
    
            return ParentDistributionBoard is ThreePhaseDistributionBoard
            {
                ThreePhaseConfiguration: ThreePhaseConfiguration.Wye
            }
                ? 1
                : 2;
        }
    }
    
    public CalculationResult<int> AmpereFrame => DataUtils.GetAmpereFrame(AmpereTrip);
    
    public int ConductorWireCount => LineToLineVoltage == LineToLineVoltage.Abc ? 3 : 2;
    
    public ConductorType? ConductorType => ConductorType.FindByIdOrNull(ConductorTypeId);
    
    public CalculationResult<double> InitialConductorSize =>
        ConductorSizeTable.GetConductorSize(ConductorType!, AmpereTrip, SetCount, 0, false);

    public virtual CalculationResult<double> ConductorSize =>
        VoltageDropCorrectionConductorSize is null
            ? InitialConductorSize
            : CalculationResult<double>.Success(VoltageDropCorrectionConductorSize.Value);
    
    public virtual string ConductorTextDisplay => ConductorSize.HasError ? 
        ConductorSize.ErrorMessage :
        $"{ConductorWireCount}-{ConductorSize} mm\u00b2 {ConductorType}";
    
    public const int GroundingWireCount = 1;
    
    public ConductorType? Grounding => ConductorType.FindByIdOrNull(GroundingId);
    
    public virtual CalculationResult<double> GroundingSize =>
        CircuitAndSubBoardGroundingSizeTable.GetGroundingSize(Grounding!.Material, AmpereTrip);
    
    public virtual string GroundingTextDisplay => GroundingSize.HasError ? 
        GroundingSize.ErrorMessage :
        $"{GroundingWireCount}-{GroundingSize} mm\u00b2 {Grounding}";
    
    public int WireCount => SetCount * (ConductorWireCount + GroundingWireCount);

    public virtual CalculationResult<double?> R => VoltageDropTable.GetR(RacewayType, ConductorType!.Material, ConductorSize);
    
    public virtual CalculationResult<double?> X => VoltageDropTable.GetX(RacewayType, ConductorSize); 
    
    public virtual CalculationResult<double?> VoltageDrop => 
        VoltageDropTable.GetVoltageDrop(
            LineToLineVoltage,
            R,
            X,
            AmpereLoad,
            WireLength,
            SetCount,
            Voltage
        );

    public virtual bool CanCorrectVoltageDropWithConductorSize
    {
        get
        {
            var currentVoltageDropCorrectionConductorSize = VoltageDropCorrectionConductorSize;
            
            var newVoltageDropCorrectionConductorSize =
                DataUtils.GetVoltageDropCorrectionConductorSize
                (
                    LineToLineVoltage,
                    RacewayType,
                    ConductorType!,
                    InitialConductorSize,
                    AmpereLoad,
                    WireLength,
                    SetCount,
                    Voltage
                );
            
            if (newVoltageDropCorrectionConductorSize.HasError || newVoltageDropCorrectionConductorSize.Value is null)
            {
                return false;
            }
            
            if (currentVoltageDropCorrectionConductorSize is null) return true;
            if (currentVoltageDropCorrectionConductorSize.Value.IsRoughlyEqualTo(newVoltageDropCorrectionConductorSize.Value.Value))
            {
                return false;
            }
            
            return currentVoltageDropCorrectionConductorSize.Value < newVoltageDropCorrectionConductorSize.Value;
        }
    }

    public virtual CalculationResult<int> RacewaySize
    {
        get
        {
            if (RacewayType == RacewayType.CableTray)
                return CableTrayRacewaySizeTable.GetCableTrayRacewaySize(
                    SetCount,
                    ConductorWireCount,
                    ConductorSize,
                    GroundingWireCount,
                    GroundingSize
                );
            
            var wireCount = ConductorWireCount + GroundingWireCount;
            return RacewaySizeTable.GetRacewaySize(
                ConductorType.WireType,
                RacewayType,
                ConductorSize,
                wireCount,
                SetCount
            );
        }
    }
    
    public virtual string RacewayTextDisplay => RacewaySize.HasError 
        ? RacewaySize.ErrorMessage
        : $"{RacewaySize} mm ø {RacewayType.GetDisplayName()}";

    public void AdjustConductorSizeForVoltageDropCorrection()
    {
        var newVoltageDropCorrectionConductorSize =
            DataUtils.GetVoltageDropCorrectionConductorSize
            (
                LineToLineVoltage,
                RacewayType,
                ConductorType!,
                InitialConductorSize,
                AmpereLoad,
                WireLength,
                SetCount,
                Voltage
            );
        
        if (newVoltageDropCorrectionConductorSize.HasError) return;
        VoltageDropCorrectionConductorSize = newVoltageDropCorrectionConductorSize.Value;
    }
    
    public void UpdateVoltageDropCorrectionConductorSize()
    {
        if (VoltageDropCorrectionConductorSize is null) return;
        
        if 
        (
            (InitialConductorSize.HasError) ||
            (
                InitialConductorSize.Value.IsRoughlyEqualTo(VoltageDropCorrectionConductorSize.Value) || 
                InitialConductorSize.Value >= VoltageDropCorrectionConductorSize.Value
            )
        )
        {
            VoltageDropCorrectionConductorSize = null;
        }

        var newR = VoltageDropTable.GetR(RacewayType, ConductorType!.Material, InitialConductorSize);
        var newX = VoltageDropTable.GetX(RacewayType, InitialConductorSize);
        var newVoltageDrop = VoltageDropTable.GetVoltageDrop(
            LineToLineVoltage,
            newR,
            newX,
            AmpereLoad,
            WireLength,
            SetCount,
            Voltage
        );

        if (!newVoltageDrop.HasError && newVoltageDrop.Value is not null && newVoltageDrop.Value < 0.03)
        {
            VoltageDropCorrectionConductorSize = null;
        }
    }
    
    public void AdjustSetCountForSizes()
    {
        while (ConductorSize.ErrorType == CalculationErrorType.NoFittingAmpereTripForConductorSize)
        {
            SetCount += 1;
        }
    }
}