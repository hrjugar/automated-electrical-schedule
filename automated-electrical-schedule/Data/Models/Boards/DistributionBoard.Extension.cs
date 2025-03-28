using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;
using automated_electrical_schedule.Data.Records;
using automated_electrical_schedule.Data.Wrappers;
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
    
    public BuildingClassification[] AllowedBuildingClassifications
    {
        get
        {
            if (Phase == BoardPhase.SinglePhase)
            {
                return [
                    BuildingClassification.DwellingUnit,
                    BuildingClassification.Other
                ];
            }
            
            return
            [
                BuildingClassification.DwellingUnit,
                BuildingClassification.Hospital,
                BuildingClassification.HotelMotelApartment,
                BuildingClassification.Warehouse,
                BuildingClassification.Other
            ];
        }
    }

    public static RacewayType[] AllowedRacewayTypes
    {
        get
        {
            var racewayTypes = Enum.GetValues<RacewayType>().ToList();
            return racewayTypes.Where(r => r != RacewayType.None).ToArray();
        }
    }

    public abstract List<BoardVoltage> AllowedVoltages { get; }

    public abstract List<LineToLineVoltage> AllowedLineToLineVoltages { get; }

    public List<TCircuit> FilterNestedCircuits<TCircuit>(Func<TCircuit, bool>? filterCallback = null) where TCircuit : Circuit
    {
        var notNullFilterCallback = filterCallback ?? (_ => true);
        
        List<TCircuit> nestedCircuits = [];
        nestedCircuits.AddRange(Circuits.OfType<TCircuit>().Where(notNullFilterCallback));
        
        foreach (var subBoard in SubDistributionBoards)
        {
            nestedCircuits.AddRange(subBoard.FilterNestedCircuits(filterCallback));
        }

        return nestedCircuits;
    }
    
    public int LastOrder
    {
        get
        {
            var circuitsLastOrder = Circuits.Select(c => c.Order).DefaultIfEmpty(0).Max();
            var subBoardsLastOrder = SubDistributionBoards.Select(b => b.Order).DefaultIfEmpty(0).Max();
            return Math.Max(circuitsLastOrder, subBoardsLastOrder);
        }
    }

    public void DecreaseOrderOfSucceedingChildren(int order)
    {
        var circuitsToDecreaseOrder = Circuits.Where(circuit => circuit.Order > order).ToList();
        var subBoardsToDecreaseOrder = SubDistributionBoards.Where(subBoard => subBoard.Order > order).ToList();

        foreach (var circuit in circuitsToDecreaseOrder)
        {
            circuit.Order -= 1;
        }

        foreach (var circuit in subBoardsToDecreaseOrder)
        {
            circuit.Order -= 1;
        }
    }

    public IOrdered FindChildByOrder(int order)
    {
        var circuit = Circuits.FirstOrDefault(c => c.Order == order);
        if (circuit is not null) return circuit;

        var subBoard = SubDistributionBoards.FirstOrDefault(b => b.Order == order);
        if (subBoard is null) throw new ArgumentOutOfRangeException(nameof(order));
        return subBoard;
    }

    public (IOrdered, IOrdered)? DecreaseChildOrder(int order)
    {
        if (order == 1) return null;
        
        var currentChild = FindChildByOrder(order);
        var previousChild = FindChildByOrder(order - 1);

        currentChild.Order -= 1;
        previousChild.Order += 1;

        return (previousChild, currentChild);
    }

    public (IOrdered, IOrdered)? IncreaseChildOrder(int order)
    {
        if (order == LastOrder) return null;

        var currentChild = FindChildByOrder(order);
        var nextChild = FindChildByOrder(order + 1);

        currentChild.Order += 1;
        nextChild.Order -= 1;

        return (currentChild, nextChild);
    }

    public List<IOrdered> CircuitsAndSubBoards => 
        Circuits
            .Cast<IOrdered>()
            .Concat(SubDistributionBoards)
            .OrderBy(e => e.Order)
            .ToList();
    
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

    // public double VoltAmpere
    // {
    //     get
    //     {
    //         // TODO: Edit volt ampere calculation
    //         return Circuits.Select(circuit => circuit.VoltAmpere).ToList().Sum() +
    //                SubDistributionBoards.Sum(subBoard => subBoard.VoltAmpere);
    //     }
    // }

    public double HighestMotorLoad =>
        Circuits
            .OfType<MotorOutletCircuit>()
            .Select(mc => mc.AmpereLoad.Value)
            .DefaultIfEmpty(0)
            .Max();


    public double VoltAmpere
    {
        get
        {
            return
                Circuits
                    .OfType<SpareCircuit>()
                    .Sum(s => s.VoltAmpere) +
                Circuits
                    .OfType<NonSpareCircuit>()
                    .Select(c => c.VoltAmpere)
                    .Sum() +
                SubDistributionBoards
                    .Select(b => b.VoltAmpere)
                    .Sum();
        }
    }
    
    public double VoltAmpereWithDemandFactor
    {
        get
        {
            double current = 0;

            var lightingCircuitsVoltAmpere = FilterVoltAmpere<LightingOutletCircuit>();
            var convenienceCircuitsVoltAmpere = FilterVoltAmpere<ConvenienceOutletCircuit>();

            var dryers = FilterNestedCircuits<ApplianceEquipmentOutletCircuit>(
                c => c.ApplianceType == ApplianceType.Dryer
            );
            var kitchenEquipments = FilterNestedCircuits<ApplianceEquipmentOutletCircuit>(
                c => c.ApplianceType == ApplianceType.KitchenEquipment
            );
            var otherApplianceEquipments = FilterNestedCircuits<ApplianceEquipmentOutletCircuit>(
                c => c.ApplianceType == ApplianceType.Other
            );
            
            var elevatorFeeders = FilterNestedCircuits<MotorOutletCircuit>(
                c => c.MotorApplication == MotorApplication.ElevatorFeeder
            );

            var cranesAndHoists = FilterNestedCircuits<MotorOutletCircuit>(
                c => c.MotorApplication == MotorApplication.CranesAndHoist
            );
            
            var normalMotors = FilterNestedCircuits<MotorOutletCircuit>(
                c => c.MotorApplication == MotorApplication.NormalMotor
            );

            var fullLoadHvacUnits = FilterNestedCircuits<MotorOutletCircuit>(
                c => c.MotorApplication == MotorApplication.FullLoadHvac
            );

            var groupedHvacUnits = FilterNestedCircuits<MotorOutletCircuit>(
                c => c.MotorApplication == MotorApplication.GroupedHvac
            );
            

            if (BuildingClassification == BuildingClassification.DwellingUnit)
            {
                current += DemandFactorFormulas.ApplyDemandFactorToDwellingUnitLightingAndConvenienceCircuits(
                    lightingCircuitsVoltAmpere + convenienceCircuitsVoltAmpere
                ).Sum();

                current += DemandFactorFormulas.ApplyDemandFactorToDwellingUnitKitchenEquipment(kitchenEquipments).Sum();
            }
            else
            {
                var lightingCurrent = BuildingClassification switch
                {
                    BuildingClassification.Hospital => DemandFactorFormulas.ApplyDemandFactorToHospitalLightingCircuits(
                        lightingCircuitsVoltAmpere
                    ).Sum(),
                    BuildingClassification.HotelMotelApartment => DemandFactorFormulas.ApplyDemandFactorToHotelMotelApartmentLightingCircuits(
                        lightingCircuitsVoltAmpere
                    ).Sum(),
                    BuildingClassification.Warehouse => DemandFactorFormulas.ApplyDemandFactorToWarehouseLightingCircuits(
                        lightingCircuitsVoltAmpere
                    ).Sum(),
                    BuildingClassification.Other => lightingCircuitsVoltAmpere,
                    BuildingClassification.DwellingUnit or _ => throw new ArgumentOutOfRangeException(nameof(BuildingClassification))
                };

                current += lightingCurrent;
                
                current +=
                    DemandFactorFormulas.ApplyDemandFactorToNonDwellingConvenienceCircuits(
                        convenienceCircuitsVoltAmpere
                    ).Sum();

                current +=
                    DemandFactorFormulas.ApplyDemandFactorToNonDwellingUnitKitchenEquipment(kitchenEquipments);
            }

            current += DemandFactorFormulas.ApplyDemandFactorToDryers(dryers);
            current += otherApplianceEquipments.Sum(aec => aec.VoltAmpere.Value);
            
            current += DemandFactorFormulas.ApplyDemandFactorToElevatorFeeders(elevatorFeeders);
            current += DemandFactorFormulas.ApplyDemandFactorToCranesAndHoists(cranesAndHoists);
            current += normalMotors.Sum(mc => mc.VoltAmpere.Value);
            current += fullLoadHvacUnits.Sum(mc => mc.VoltAmpere.Value);
            current += DemandFactorFormulas.ApplyDemandFactorToGroupedHvacUnits(groupedHvacUnits);

            current += FilterVoltAmpere<SpareCircuit>();

            return current;
        }
    }

    public double FilterVoltAmpere<TCircuit>(Func<TCircuit, bool>? filterCallback = null) where TCircuit : NonSpaceCircuit
    {
        var nestedCircuits = FilterNestedCircuits<TCircuit>(filterCallback);
        
        if (typeof(TCircuit).IsSubclassOf(typeof(NonSpareCircuit)))
        {
            return nestedCircuits
                .OfType<NonSpareCircuit>()
                .Select(circuit => circuit.VoltAmpere)
                .Sum();
        }
        return
            nestedCircuits
                .OfType<SpareCircuit>()
                .Select(circuit => circuit.VoltAmpere)
                .Sum();
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
    
    public bool HasNestedCircuits =>
        Circuits.Count > 0 || SubDistributionBoards.Any(subBoard => subBoard.HasNestedCircuits);

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
            if (LineToLineVoltage == LineToLineVoltage.Abc) return 3;

            return ParentDistributionBoard is ThreePhaseDistributionBoard
            {
                ThreePhaseConfiguration: ThreePhaseConfiguration.Wye
            }
                ? 1
                : 2;
        }
    }

    public int TotalChildPoles =>
        Circuits.OfType<NonSpaceCircuit>().Select(c => c.Pole).Sum() +
        SubDistributionBoards.Select(b => b.Pole).Sum();
    
    public const int RecommendedTotalChildPoles = 48;

    // public CalculationResult<double> Current => CalculationResult<double>.Success(VoltAmpere / (int) Voltage);
    public CalculationResult<double> Current
    {
        get
        {
            var denominator = Phase == BoardPhase.ThreePhase ? (int) Voltage * Math.Sqrt(3) : (int) Voltage;
            return CalculationResult<double>.Success(VoltAmpereWithDemandFactor / denominator);
        }
    }

    public double CircuitProtectionAmpere => Current.Value + (HighestMotorLoad * 1.25);

    public double ConductorAmpere => Current.Value + (HighestMotorLoad * 0.25);

    public CalculationResult<int> AmpereTrip
    {
        get
        {
            if (!HasNestedCircuits) return CalculationResult<int>.Failure(CalculationErrorType.NoCircuits);
            
            return Current.HasError 
                ? CalculationResult<int>.Failure(Current.ErrorType) 
                : DataUtils.GetAmpereTrip(CalculationResult<double>.Success(CircuitProtectionAmpere), 20);
        }
    }

    public abstract CalculationResult<double> AmpereLoad { get; }

    public CalculationResult<int> AmpereFrame
    {
        get
        {
            return AmpereTrip.HasError
                ? CalculationResult<int>.Failure(AmpereTrip.ErrorType)
                : DataUtils.GetAmpereFrame(AmpereTrip);
        }
    }

    public CalculationResult<double?> R => VoltageDropTable.GetR(RacewayType, ConductorType.Material, ConductorSize);

    public CalculationResult<double?> X => VoltageDropTable.GetX(RacewayType, ConductorSize);

    public CalculationResult<double?> VoltageDrop => 
        VoltageDropTable.GetVoltageDrop(
            this is ThreePhaseDistributionBoard threePhaseBoard ? threePhaseBoard.LineToLineVoltage : LineToLineVoltage.None,
            R,
            X,
            CalculationResult<double>.Success(ConductorAmpere), 
            WireLength,
            SetCount,
            (int)Voltage
        );
    
    public bool CanCorrectVoltageDropWithConductorSize
    {
        get
        {
            var currentVoltageDropCorrectionConductorSize = VoltageDropCorrectionConductorSize;

            var newVoltageDropCorrectionConductorSize =
                DataUtils.GetVoltageDropCorrectionConductorSize
                (
                    LineToLineVoltage,
                    RacewayType,
                    ConductorType,
                    TemperatureAffectedConductorSize,
                    AmpereLoad,
                    WireLength,
                    SetCount,
                    (int)Voltage
                );

            if (newVoltageDropCorrectionConductorSize.HasError || newVoltageDropCorrectionConductorSize.Value is null)
            {
                return false;
            }
            
            if (currentVoltageDropCorrectionConductorSize is null) return true;

            return currentVoltageDropCorrectionConductorSize.Value < newVoltageDropCorrectionConductorSize.Value;
        }
    }

    public ConductorType ConductorType => ConductorType.FindById(ConductorTypeId);

    private double MinimumConductorSize =>
        ParentDistributionBoard is null ? 
            ConductorType.Material == ConductorMaterial.Copper ? 
                8.0 : 
                14.0 : 
            3.5;
    
    public CalculationResult<double> InitialConductorSize => 
        ConductorSizeTable.GetConductorSize(ConductorType, AmpereTrip, SetCount, MinimumConductorSize);

    public CalculationResult<int> InitialConductorSizeAmpacity =>
        ConductorSizeTable.GetAmpacity(
            InitialConductorSize,
            ConductorType
        );
    
    public string InitialConductorTextDisplay => InitialConductorSize.HasError ? 
        ConductorSize.ErrorMessage :
        $"{ConductorWireCount}-{InitialConductorSize} mm\u00b2 {ConductorType}";
    
    public double AmbientTemperatureMultiplier =>
        AmbientTemperatureTable.GetAmbientTemperatureMultiplier(
            AmbientTemperature,
            ConductorType.TemperatureRating
        );
    
    public CalculationResult<double> TemperatureAffectedConductorSize
    {
        get
        {
            if (InitialConductorSizeAmpacity.HasError) return CalculationResult<double>.Failure(InitialConductorSizeAmpacity.ErrorType);

            var ampacityWithMultiplier = CalculationResult<int>.Success(
                (int) (InitialConductorSizeAmpacity.Value * AmbientTemperatureMultiplier)
            );

            return ConductorSizeTable.GetConductorSize(ConductorType, ampacityWithMultiplier, SetCount, MinimumConductorSize);
        }
    }
    
    public string TemperatureAffectedConductorTextDisplay => TemperatureAffectedConductorSize.HasError ? 
        TemperatureAffectedConductorSize.ErrorMessage :
        $"{ConductorWireCount}-{TemperatureAffectedConductorSize} mm\u00b2 {ConductorType}";

    public CalculationResult<double> ConductorSize =>
        VoltageDropCorrectionConductorSize is null
            ? TemperatureAffectedConductorSize
            : CalculationResult<double>.Success(VoltageDropCorrectionConductorSize.Value);

    public int ConductorWireCount
    {
        get
        {
            return this switch
            {
                SinglePhaseDistributionBoard => 2,
                ThreePhaseDistributionBoard threePhaseBoard => threePhaseBoard.ThreePhaseConfiguration switch
                {
                    ThreePhaseConfiguration.Delta => 3,
                    ThreePhaseConfiguration.Wye => 4,
                    _ => throw new ArgumentOutOfRangeException(nameof(threePhaseBoard.ThreePhaseConfiguration))
                },
                _ => throw new ArgumentOutOfRangeException(nameof(ParentDistributionBoard))
            };
        }
    }
    
    public string ConductorTextDisplay => ConductorSize.HasError ? 
        ConductorSize.ErrorMessage :
        $"{ConductorWireCount}-{ConductorSize} mm\u00b2 {ConductorType}";
    
    public int WireCount => SetCount * (ConductorWireCount + GroundingWireCount);

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
    
    public string GroundingTextDisplay => GroundingSize.HasError ? 
        GroundingSize.ErrorMessage :
        $"{GroundingWireCount}-{GroundingSize} mm\u00b2 {Grounding}";

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

    public string RacewayTextDisplay => $"{RacewaySize} mm ø {RacewayType.GetDisplayName()}";

    // private CalculationResult<double> TransformerCurrent => HasTransformer
    //             ? CalculationResult<double>.Success(Math.Sqrt(3) * (int)Voltage * Current)
    //             : CalculationResult<double>.Failure(CalculationErrorType.NoTransformer);

    private CalculationResult<double> TransformerCurrent
    {
        get
        {
            if (!HasTransformer) return CalculationResult<double>.Failure(CalculationErrorType.NoTransformer);
            if (!HasNestedCircuits) return CalculationResult<double>.Failure(CalculationErrorType.NoCircuits);
            if (Current.HasError) return CalculationResult<double>.Failure(Current.ErrorType);
            return CalculationResult<double>.Success(Math.Sqrt(3) * (int)Voltage * Current.Value);
        }
    }

    public CalculationResult<int> TransformerRating => 
        TransformerCurrent.HasError
            ? CalculationResult<int>.Failure(TransformerCurrent.ErrorType)
            : TransformerTable.GetTransformerRating(TransformerCurrent);

    public double TransformerPrimaryProtectionAmpereWithoutMultiplier
    {
        get
        {
            var voltage = ParentDistributionBoard is null ? 13800 : (int)ParentDistributionBoard.Voltage;
            var denominatorMultiplier = Phase == BoardPhase.ThreePhase ? Math.Sqrt(3) : 1;

            return VoltAmpereWithDemandFactor / (denominatorMultiplier * voltage);
        }
    }
    public double TransformerPrimaryProtectionMultiplier =>
        ParentDistributionBoard is null
            ? TransformerTable.MainBoardTransformerPrimaryProtectionFactor
            : TransformerTable.SubBoardTransformerPrimaryProtectionFactor;

    public double TransformerPrimaryProtectionAmpere =>
        TransformerPrimaryProtectionAmpereWithoutMultiplier * TransformerPrimaryProtectionMultiplier;

    public CalculationResult<int> TransformerPrimaryProtectionAmpereTrip => 
        DataUtils.GetAmpereTrip(
            CalculationResult<double>.Success(TransformerPrimaryProtectionAmpere)
        );
    
    public double TransformerSecondaryProtectionAmpereWithoutMultiplier
    {
        get
        {
            var denominatorMultiplier = Phase == BoardPhase.ThreePhase ? Math.Sqrt(3) : 1;

            return VoltAmpereWithDemandFactor / (denominatorMultiplier * (int)Voltage);
        }
    }
    
    public double TransformerSecondaryProtectionMultiplier =>
        ParentDistributionBoard is null
            ? TransformerTable.MainBoardTransformerSecondaryProtectionFactor
            : Current.Value >= 9
                ? TransformerTable.SubBoardTransformerSecondaryProtectionGreaterEqual9
                : TransformerTable.SubBoardTransformerSecondaryProtectionLessThan9;

    public double TransformerSecondaryProtectionAmpere => 
        TransformerSecondaryProtectionAmpereWithoutMultiplier * TransformerSecondaryProtectionMultiplier;
    
    public CalculationResult<int> TransformerSecondaryProtectionAmpereTrip => 
        DataUtils.GetAmpereTrip(
            CalculationResult<double>.Success(TransformerSecondaryProtectionAmpere)
        );

    public ConductorType? BreakerConductorType =>
        BreakerConductorTypeId is null ? null : ConductorType.FindById(BreakerConductorTypeId);

    public ConductorType? BreakerGrounding =>
        BreakerGroundingId is null ? null : ConductorType.FindById(BreakerGroundingId);
    
    public string ConductorHeaderDisplay
    {
        get
        {
            if (this is SinglePhaseDistributionBoard) return "Phase+Neutral";
            if (this is ThreePhaseDistributionBoard
                {
                    ThreePhaseConfiguration: ThreePhaseConfiguration.Delta
                }) return "Line";

            return "Line+Neutral";
        }
    }
    
    public abstract DistributionBoard Clone();
    
    public void AdjustConductorSizeForVoltageDropCorrection()
    {
        var newVoltageDropCorrectionConductorSize =
            DataUtils.GetVoltageDropCorrectionConductorSize
            (
                LineToLineVoltage,
                RacewayType,
                ConductorType,
                TemperatureAffectedConductorSize,
                AmpereLoad,
                WireLength,
                SetCount,
                (int)Voltage
            );

        if (newVoltageDropCorrectionConductorSize.HasError) return;
        CorrectedVoltageDrop = VoltageDrop.Value;
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
            CorrectedVoltageDrop = null;
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
            (int)Voltage
        );

        if (!newVoltageDrop.HasError && newVoltageDrop.Value is not null && !newVoltageDrop.Value.Value.IsRoughlyEqualTo(CorrectedVoltageDrop!.Value))
        {
            CorrectedVoltageDrop = null;
            VoltageDropCorrectionConductorSize = null;
        }
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