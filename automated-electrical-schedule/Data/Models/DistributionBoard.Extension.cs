using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class DistributionBoard
{
    public const int GroundingWireCount = 1;

    public static readonly List<CircuitProtection> AllowedCircuitProtections =
    [
        CircuitProtection.MiniatureCircuitBreaker,
        CircuitProtection.MoldedCaseCircuitBreaker,
        CircuitProtection.OilCircuitBreaker,
        CircuitProtection.AirBreakCircuitBreaker,
        CircuitProtection.AirBlastCircuitBreaker,
        CircuitProtection.VacuumCircuitBreaker,
        CircuitProtection.SulfurHexafluorideCircuitBreaker
    ];

    public List<BoardPhase> AllowedPhases
    {
        get
        {
            switch (ParentDistributionBoard)
            {
                case SinglePhaseDistributionBoard:
                    return [BoardPhase.SinglePhase];
                case ThreePhaseDistributionBoard:
                    return [BoardPhase.SinglePhase, BoardPhase.ThreePhase];
                default:
                    if (SubDistributionBoards.Count > 0 || Circuits.Count > 0) return [Phase];
                    return Enum.GetValues<BoardPhase>().ToList();
            }
        }
    }

    public abstract List<BoardVoltage> AllowedVoltages { get; }

    public abstract List<LineToLineVoltage> AllowedLineToLineVoltages { get; }

    public string LineToLineVoltageDisplay
    {
        get
        {
            if (LineToLineVoltage == LineToLineVoltage.Abc) return "ABC";
            if (LineToLineVoltage is LineToLineVoltage.None) return "";

            var currentBoard = ParentDistributionBoard;
            ThreePhaseConfiguration? threePhaseConfiguration = null;

            while (currentBoard is not null)
            {
                if (currentBoard is ThreePhaseDistributionBoard threePhaseBoard)
                {
                    threePhaseConfiguration = threePhaseBoard.ThreePhaseConfiguration;
                    break;
                }

                currentBoard = currentBoard.ParentDistributionBoard;
            }

            return threePhaseConfiguration switch
            {
                ThreePhaseConfiguration.Delta => LineToLineVoltage switch
                {
                    Enums.LineToLineVoltage.A => "AB",
                    Enums.LineToLineVoltage.B => "BC",
                    Enums.LineToLineVoltage.C => "CA",
                    _ => throw new ArgumentOutOfRangeException(nameof(LineToLineVoltage))
                },
                ThreePhaseConfiguration.Wye => LineToLineVoltage switch
                {
                    Enums.LineToLineVoltage.A => "AN",
                    Enums.LineToLineVoltage.B => "BN",
                    Enums.LineToLineVoltage.C => "CN",
                    _ => throw new ArgumentOutOfRangeException(nameof(LineToLineVoltage))
                },
                null => "XXX",
                _ => throw new ArgumentOutOfRangeException(nameof(LineToLineVoltage))
            };
            ;
        }
    }

    public double VoltAmpere
    {
        get
        {
            return Circuits.Select(circuit => circuit.VoltAmpere).ToList().Sum() +
                   SubDistributionBoards.Sum(subBoard => subBoard.VoltAmpere);
        }
    }

    public List<CircuitProtection> AllowedTransformerPrimaryProtections
    {
        get
        {
            if (ParentDistributionBoard is null && this is ThreePhaseDistributionBoard)
                return [CircuitProtection.CutOutFuse];

            return AllowedCircuitProtections;
        }
    }
    
    public bool HasCircuitsRecursive =>
        Circuits.Count > 0 || SubDistributionBoards.Any(subBoard => subBoard.HasCircuitsRecursive);

    public bool HasBreaker =>
        ParentDistributionBoard is not null &&
        (int)ParentDistributionBoard.Voltage > (int)Voltage &&
        ParentDistributionBoard is ThreePhaseDistributionBoard
        {
            ThreePhaseConfiguration : ThreePhaseConfiguration.Delta
        };

    public bool HasTransformer =>
        (ParentDistributionBoard is null && this is ThreePhaseDistributionBoard) || HasBreaker;

    public int Pole
    {
        get
        {
            if (LineToLineVoltage == Enums.LineToLineVoltage.Abc) return 3;

            if (ParentDistributionBoard is ThreePhaseDistributionBoard parentThreePhaseBoard &&
                parentThreePhaseBoard.ThreePhaseConfiguration == ThreePhaseConfiguration.Delta)
                return 2;

            return 1;
        }
    }

    protected abstract CalculationResult<double> Current { get; }

    public CalculationResult<int> AmpereTrip
    {
        get
        {
            if (!HasCircuitsRecursive) return CalculationResult<int>.Failure(CalculationErrorType.NoCircuits);
            
            return Current.HasError 
                ? CalculationResult<int>.Failure(Current.ErrorType) 
                : DataUtils.GetAmpereTrip(CalculationResult<double>.Success(Current.Value / 0.8), 20);
        }
    }

    public abstract double AmpereLoad { get; }

    public CalculationResult<int> AmpereFrame
    {
        get
        {
            return AmpereTrip.HasError
                ? CalculationResult<int>.Failure(AmpereTrip.ErrorType)
                : DataUtils.GetAmpereFrame(AmpereTrip);
        }
    }

    protected CalculationResult<double> R => 
        RacewayType == RacewayType.CableTray
            ? CalculationResult<double>.Success(0)
            : VoltageDropTable.GetR(RacewayType, ConductorType.Material, ConductorSize);

    protected CalculationResult<double> X => 
        RacewayType == RacewayType.CableTray
            ? CalculationResult<double>.Success(0)
            : VoltageDropTable.GetX(RacewayType, ConductorSize);

    public CalculationResult<double> VoltageDrop
    {
        get
        {
            if (RacewayType == RacewayType.CableTray || WireLength is null) return CalculationResult<double>.Success(0);

            return VoltageDropTable.GetVoltageDrop(
                this is ThreePhaseDistributionBoard threePhaseBoard ? threePhaseBoard.LineToLineVoltage : LineToLineVoltage.None,
                R,
                X,
                CalculationResult<double>.Success(AmpereLoad),
                WireLength.Value,
                SetCount,
                (int)Voltage
            );
        }
    }

    public ConductorType ConductorType => ConductorType.FindById(ConductorTypeId);

    public CalculationResult<double> ConductorSize
    {
        get
        {
            double minConductorSize;
            if (ParentDistributionBoard is null)
                minConductorSize = ConductorType.Material == ConductorMaterial.Copper ? 8.0 : 14.0;
            else
                minConductorSize = 3.5;

            return ConductorSizeTable.GetConductorSize(ConductorType, AmpereTrip, SetCount, minConductorSize);
        }
    }

    public int ConductorWireCount => LineToLineVoltage == Enums.LineToLineVoltage.Abc ? 3 : 2;

    public ConductorType Grounding => ConductorType.FindById(GroundingId);

    // public CalculationResult<double> GroundingSize => ParentDistributionBoard is null
    //     ? MainBoardGroundingSizeTable.GetGroundingSize(ConductorType.Material, Grounding.Material,
    //         ConductorSize)
    //     : CircuitAndSubBoardGroundingSizeTable.GetGroundingSize(Grounding.Material, AmpereTrip);

    public CalculationResult<double> GroundingSize
    {
        get
        {
            if (ParentDistributionBoard is null)
                return MainBoardGroundingSizeTable.GetGroundingSize(ConductorType.Material, Grounding.Material,
                    ConductorSize);
            
            var conductorSizeWithoutSetCountIncluded = 
                ConductorSizeTable.GetConductorSize(ConductorType, AmpereTrip, SetCount, 3.5);
            
            return
                CircuitAndSubBoardGroundingSizeTable.GetGroundingSize(
                    Grounding.Material, 
                    AmpereTrip,
                    conductorSizeWithoutSetCountIncluded.ErrorType == CalculationErrorType.NoFittingAmpereTripForConductorSize 
                        ? SetCount 
                        : 1
                );
        }
    }

    public CalculationResult<int> RacewaySize
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

    // private CalculationResult<double> TransformerCurrent => HasTransformer
    //             ? CalculationResult<double>.Success(Math.Sqrt(3) * (int)Voltage * Current)
    //             : CalculationResult<double>.Failure(CalculationErrorType.NoTransformer);

    private CalculationResult<double> TransformerCurrent
    {
        get
        {
            if (!HasTransformer) return CalculationResult<double>.Failure(CalculationErrorType.NoTransformer);
            if (!HasCircuitsRecursive) return CalculationResult<double>.Failure(CalculationErrorType.NoCircuits);
            if (Current.HasError) return CalculationResult<double>.Failure(Current.ErrorType);
            return CalculationResult<double>.Success(Math.Sqrt(3) * (int)Voltage * Current.Value);
        }
    }

    public CalculationResult<int> TransformerRating => 
        TransformerCurrent.HasError
            ? CalculationResult<int>.Failure(TransformerCurrent.ErrorType)
            : TransformerTable.GetTransformerRating(TransformerCurrent);
    
    public CalculationResult<int> TransformerPrimaryProtectionAmpereTrip
    {
        get
        {
            if (TransformerCurrent.HasError) return CalculationResult<int>.Failure(TransformerCurrent.ErrorType);

            var primaryProtectionFactor = ParentDistributionBoard is null
                ? TransformerTable.MainBoardTransformerPrimaryProtectionFactor
                : TransformerTable.SubBoardTransformerPrimaryProtectionFactor;
            
            var value = CalculationResult<double>.Success(
                TransformerCurrent.Value / (Math.Sqrt(3) * (int)Voltage) * primaryProtectionFactor
            );
            
            return DataUtils.GetAmpereTrip(value);
        }
    }

    public CalculationResult<int> TransformerSecondaryProtectionAmpereTrip
    {
        get
        {
            if (TransformerCurrent.HasError) return CalculationResult<int>.Failure(TransformerCurrent.ErrorType);
            if (Current.HasError) return CalculationResult<int>.Failure(Current.ErrorType);
            
            var secondaryProtectionFactor = ParentDistributionBoard is null
                ? TransformerTable.MainBoardTransformerSecondaryProtectionFactor
                : Current.Value >= 9
                    ? TransformerTable.SubBoardTransformerSecondaryProtectionGreaterEqual9
                    : TransformerTable.SubBoardTransformerSecondaryProtectionLessThan9;
            
            var value = CalculationResult<double>.Success(Current.Value * secondaryProtectionFactor);
            return DataUtils.GetAmpereTrip(value);
        }
    }

    public ConductorType? BreakerConductorType =>
        BreakerConductorTypeId is null ? null : ConductorType.FindById(BreakerConductorTypeId);

    public ConductorType? BreakerGrounding =>
        BreakerGroundingId is null ? null : ConductorType.FindById(BreakerGroundingId);

    public abstract DistributionBoard Clone();
    
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