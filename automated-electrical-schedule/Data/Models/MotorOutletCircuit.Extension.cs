using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;

namespace automated_electrical_schedule.Data.Models;

public partial class MotorOutletCircuit
{
    public List<MotorType> AllowedMotorTypes => GetAllowedMotorTypesStatic(ParentDistributionBoard, LineToLineVoltage);

    public List<double> AllowedHorsepowerValues => GetAllowedHorsepowerValuesStatic(MotorType, LineToLineVoltage);

    public override List<CircuitProtection> AllowedCircuitProtections =>
    [
        CircuitProtection.NonTimeDelayFuse,
        CircuitProtection.DualElement,
        CircuitProtection.InstantaneousTripBreaker,
        CircuitProtection.InverseTimeBreaker
    ];

    public override CalculationResult<double> VoltAmpere => 
        AmpereLoad.HasError
            ? CalculationResult<double>.Failure(AmpereLoad.ErrorType)
            : CalculationResult<double>.Success(Voltage * AmpereLoad.Value);

    public override CalculationResult<double> AmpereLoad => ParentDistributionBoard.Phase == BoardPhase.SinglePhase ||
                                                            MotorType == MotorType.SinglePhaseMotor
        ? DataUtils.GetMotorOutlet230VoltAmpereLoad(Horsepower)
        : ThreePhaseMotorLoadTable.GetMotorLoad(ParentDistributionBoard.Voltage, MotorType, Horsepower);

    public override CalculationResult<int> AmpereTrip
    {
        get
        {
            if (AmpereLoad.HasError) return CalculationResult<int>.Failure(AmpereLoad.ErrorType);
            if (MotorType == MotorType.InductionMotorFirePump) return DataUtils.GetAmpereTrip(CalculationResult<double>.Success(AmpereLoad.Value / 0.8));

            double factor;
            switch (CircuitProtection)
            {
                case CircuitProtection.NonTimeDelayFuse:
                    switch (MotorType)
                    {
                        case MotorType.SinglePhaseMotor:
                        case MotorType.SquirrelCage:
                        case MotorType.DesignBEnergyEfficient:
                        case MotorType.Synchronous:
                            factor = 3;
                            break;
                        case MotorType.WoundRotor:
                            factor = 1.5;
                            break;
                        case MotorType.InductionMotorFirePump:
                        default:
                            return CalculationResult<int>.Failure(CalculationErrorType.InvalidMotorType);
                    }
                    
                    break;
                case CircuitProtection.DualElement:
                    switch (MotorType)
                    {
                        case MotorType.SinglePhaseMotor:
                        case MotorType.SquirrelCage:
                        case MotorType.DesignBEnergyEfficient:
                        case MotorType.Synchronous:
                            factor = 1.75;
                            break;
                        case MotorType.WoundRotor:
                            factor = 1.5;
                            break;
                        case MotorType.InductionMotorFirePump:
                        default:
                            return CalculationResult<int>.Failure(CalculationErrorType.InvalidMotorType);
                    }

                    break;
                case CircuitProtection.InstantaneousTripBreaker:
                    switch (MotorType)
                    {
                        case MotorType.DesignBEnergyEfficient:
                            factor = 11;
                            break;
                        case MotorType.SinglePhaseMotor:
                        case MotorType.SquirrelCage:
                        case MotorType.Synchronous:
                        case MotorType.WoundRotor:
                            factor = 8;
                            break;
                        case MotorType.InductionMotorFirePump:
                        default:
                            return CalculationResult<int>.Failure(CalculationErrorType.InvalidMotorType);
                    }

                    break;
                case CircuitProtection.InverseTimeBreaker:
                    switch (MotorType)
                    {
                        case MotorType.SinglePhaseMotor:
                        case MotorType.SquirrelCage:
                        case MotorType.DesignBEnergyEfficient:
                        case MotorType.Synchronous:
                            factor = 2.5;
                            break;
                        case MotorType.WoundRotor:
                            factor = 1.5;
                            break;
                        case MotorType.InductionMotorFirePump:
                        default:
                            return CalculationResult<int>.Failure(CalculationErrorType.InvalidMotorType);
                    }
                    break;
                default:
                    return CalculationResult<int>.Failure(CalculationErrorType.InvalidCircuitProtection);
            }
            
            // var factor = CircuitProtection switch
            // {
            //     CircuitProtection.NonTimeDelayFuse =>
            //         MotorType switch
            //         {
            //             MotorType.SinglePhaseMotor or
            //                 MotorType.SquirrelCage or
            //                 MotorType.DesignBEnergyEfficient or
            //                 MotorType.Synchronous => 3,
            //             MotorType.WoundRotor => 1.5,
            //             _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
            //         },
            //     CircuitProtection.DualElement =>
            //         MotorType switch
            //         {
            //             MotorType.SinglePhaseMotor or
            //                 MotorType.SquirrelCage or
            //                 MotorType.DesignBEnergyEfficient or
            //                 MotorType.Synchronous => 1.75,
            //             MotorType.WoundRotor => 1.5,
            //             _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
            //         },
            //     CircuitProtection.InstantaneousTripBreaker =>
            //         MotorType switch
            //         {
            //             MotorType.DesignBEnergyEfficient => 11,
            //             MotorType.SinglePhaseMotor or
            //                 MotorType.SquirrelCage or
            //                 MotorType.Synchronous or
            //                 MotorType.WoundRotor => 8,
            //             _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
            //         },
            //     CircuitProtection.InverseTimeBreaker =>
            //         MotorType switch
            //         {
            //             MotorType.SinglePhaseMotor or
            //                 MotorType.SquirrelCage or
            //                 MotorType.DesignBEnergyEfficient or
            //                 MotorType.Synchronous => 2.5,
            //             MotorType.WoundRotor => 1.5,
            //             _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
            //         },
            //     _ =>
            //         throw new ArgumentOutOfRangeException(nameof(CircuitProtection))
            // };

            var value = CalculationResult<double>.Success(AmpereLoad.Value * factor);
            return DataUtils.GetAmpereTrip(value);
        }
    }

    public override Circuit Clone()
    {
        return new MotorOutletCircuit
        {
            Id = Id,
            ParentDistributionBoardId = ParentDistributionBoardId,
            ParentDistributionBoard = ParentDistributionBoard,
            CircuitType = CircuitType,
            LineToLineVoltage = LineToLineVoltage,
            Description = Description,
            Quantity = Quantity,
            WireLength = WireLength,
            DemandFactor = DemandFactor,
            CircuitProtection = CircuitProtection,
            SetCount = SetCount,
            ConductorTypeId = ConductorTypeId,
            GroundingId = GroundingId,
            RacewayType = RacewayType,

            MotorType = MotorType,
            Horsepower = Horsepower
        };
    }

    public static List<MotorType> GetAllowedMotorTypesStatic(DistributionBoard parentDistributionBoard,
        LineToLineVoltage lineToLineVoltage)
    {
        if (parentDistributionBoard.Phase == BoardPhase.SinglePhase || lineToLineVoltage != LineToLineVoltage.Abc)
            return [MotorType.SinglePhaseMotor];

        if (parentDistributionBoard.Voltage == BoardVoltage.V400)
            return
            [
                MotorType.SquirrelCage,
                MotorType.DesignBEnergyEfficient,
                MotorType.WoundRotor,
                MotorType.InductionMotorFirePump
            ];

        return
        [
            MotorType.SquirrelCage,
            MotorType.DesignBEnergyEfficient,
            MotorType.Synchronous,
            MotorType.WoundRotor,
            MotorType.InductionMotorFirePump
        ];
    }
    
    public static List<double> GetAllowedHorsepowerValuesStatic(MotorType motorType, LineToLineVoltage lineToLineVoltage)
    {
        if (lineToLineVoltage != LineToLineVoltage.Abc)
            return DataConstants.SinglePhaseHorsepowerValues;

        return motorType == MotorType.Synchronous
            ? DataConstants.SynchronousThreePhaseHorsepowerValues
            : DataConstants.GeneralThreePhaseHorsepowerValues;
    }
}