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
    
    public CalculationResult<int> AmpereFrame => DataUtils.GetAmpereFrame(AmpereTrip);
    
    public int ConductorWireCount => LineToLineVoltage == LineToLineVoltage.Abc ? 3 : 2;
    
    public ConductorType? ConductorType => ConductorType.FindByIdOrNull(ConductorTypeId);
    
    public virtual CalculationResult<double> ConductorSize =>
        ConductorSizeTable.GetConductorSize(ConductorType!, AmpereTrip, SetCount, 0, false);
    
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

    public virtual CalculationResult<double> R =>
        RacewayType == RacewayType.CableTray
            ? CalculationResult<double>.Success(0)
            : VoltageDropTable.GetR(RacewayType, ConductorType!.Material, ConductorSize);
    
    public virtual CalculationResult<double> X =>
        RacewayType == RacewayType.CableTray
            ? CalculationResult<double>.Success(0)
            : VoltageDropTable.GetX(RacewayType, ConductorSize); 
    // {
    //     get
    //     {
    //         if (RacewayType == RacewayType.CableTray)
    //         {
    //             return CalculationResult<double>.Success(0);
    //         }
    //
    //         return VoltageDropTable.GetX(RacewayType, ConductorSize);
    //     }
    // }
    
    public virtual CalculationResult<double> VoltageDrop
    {
        get
        {
            if (RacewayType == RacewayType.CableTray)
            {
                return CalculationResult<double>.Success(0);
            }
            
            return VoltageDropTable.GetVoltageDrop(
                LineToLineVoltage,
                R,
                X,
                AmpereLoad,
                WireLength,
                SetCount,
                Voltage
            );            
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
    
    public void CorrectVoltageDrop()
    {
        if (VoltageDrop.HasError) return;
        while (VoltageDrop.Value * 100 >= 3) SetCount += 1;
    }
    
    private void AdjustSetCountForConductorSize()
    {
        while (ConductorSize.ErrorType == CalculationErrorType.NoFittingAmpereTripForConductorSize)
        {
            SetCount += 1;
        }
    }
    
    public void AdjustSetCountForSizes()
    {
        AdjustSetCountForConductorSize();
    }
}