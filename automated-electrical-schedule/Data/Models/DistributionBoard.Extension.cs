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

            return current;
        }
    }

    public double FilterVoltAmpere<TCircuit>(Func<TCircuit, bool>? filterCallback = null) where TCircuit : Circuit
    {
        var nestedCircuits = FilterNestedCircuits<TCircuit>(filterCallback);
        return nestedCircuits.Select(circuit => circuit.VoltAmpere).Sum();
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
            if (LineToLineVoltage == LineToLineVoltage.Abc) return 3;

            return ParentDistributionBoard is ThreePhaseDistributionBoard
            {
                ThreePhaseConfiguration: ThreePhaseConfiguration.Wye
            }
                ? 1
                : 2;
        }
    }

    // public CalculationResult<double> Current => CalculationResult<double>.Success(VoltAmpere / (int) Voltage);
    public CalculationResult<double> Current
    {
        get
        {
            var denominator = Phase == BoardPhase.ThreePhase ? (int) Voltage * Math.Sqrt(3) : (int) Voltage;
            return CalculationResult<double>.Success(VoltAmpere / denominator);
        }
    }

    public double CircuitProtectionAmpere => Current.Value + (HighestMotorLoad * 1.25);

    public double ConductorAmpere => Current.Value + (HighestMotorLoad * 0.25);

    public CalculationResult<int> AmpereTrip
    {
        get
        {
            if (!HasCircuitsRecursive) return CalculationResult<int>.Failure(CalculationErrorType.NoCircuits);
            
            return Current.HasError 
                ? CalculationResult<int>.Failure(Current.ErrorType) 
                : DataUtils.GetAmpereTrip(CalculationResult<double>.Success(CircuitProtectionAmpere), 20);
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
    
    public double AmbientTemperatureMultiplier =>
        AmbientTemperatureTable.GetAmbientTemperatureMultiplier(
            AmbientTemperature,
            ConductorType.TemperatureRating
        );
    
    public CalculationResult<double> ConductorSize
    {
        get
        {
            if (InitialConductorSizeAmpacity.HasError) return CalculationResult<double>.Failure(InitialConductorSizeAmpacity.ErrorType);

            var ampacityWithMultiplier = InitialConductorSizeAmpacity.Value * AmbientTemperatureMultiplier;

            var ampacityWithMultiplierAmpereTrip =
                DataUtils.GetAmpereTrip(CalculationResult<double>.Success(ampacityWithMultiplier), 20);

            return ConductorSizeTable.GetConductorSize(ConductorType, ampacityWithMultiplierAmpereTrip, SetCount, MinimumConductorSize);
        }
    }

    public int ConductorWireCount => LineToLineVoltage == Enums.LineToLineVoltage.Abc ? 3 : 2;
    
    public string InitialConductorTextDisplay => InitialConductorSize.HasError ? 
        ConductorSize.ErrorMessage :
        $"{ConductorWireCount}-{InitialConductorSize} mm\u00b2 {ConductorType}";
    
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
            if (!HasCircuitsRecursive) return CalculationResult<double>.Failure(CalculationErrorType.NoCircuits);
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

            return VoltAmpere / (denominatorMultiplier * voltage);
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

            return VoltAmpere / (denominatorMultiplier * (int)Voltage);
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