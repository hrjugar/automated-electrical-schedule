using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;

namespace automated_electrical_schedule.Data.Models;

public partial class MotorOutletCircuit
{
    public List<MotorType> AllowedMotorTypes => GetAllowedMotorTypesStatic(ParentDistributionBoard, LineToLineVoltage);

    public List<double> AllowedHorsepowerValues
    {
        get
        {
            if (ParentDistributionBoard.Phase == BoardPhase.SinglePhase)
                return DataConstants.SinglePhaseHorsepowerValues;

            return MotorType == MotorType.Synchronous
                ? DataConstants.SynchronousThreePhaseHorsepowerValues
                : DataConstants.GeneralThreePhaseHorsepowerValues;
        }
    }

    public override List<CircuitProtection> AllowedCircuitProtections =>
    [
        CircuitProtection.NonTimeDelayFuse,
        CircuitProtection.DualElement,
        CircuitProtection.InstantaneousTripBreaker,
        CircuitProtection.InverseTimeBreaker
    ];

    public override double VoltAmpere => Voltage * AmpereLoad;

    public override double AmpereLoad => ParentDistributionBoard.Phase == BoardPhase.SinglePhase ||
                                         MotorType == MotorType.SinglePhaseMotor
        ? DataUtils.GetMotorOutlet230VoltAmpereLoad(Horsepower)
        : ThreePhaseMotorLoadTable.GetMotorLoad(ParentDistributionBoard.Voltage, MotorType, Horsepower);

    public override int AmpereTrip
    {
        get
        {
            if (MotorType == MotorType.InductionMotorFirePump) return DataUtils.GetAmpereTrip(AmpereLoad / 0.8);

            var factor = CircuitProtection switch
            {
                CircuitProtection.NonTimeDelayFuse =>
                    MotorType switch
                    {
                        MotorType.SinglePhaseMotor or
                            MotorType.SquirrelCage or
                            MotorType.DesignBEnergyEfficient or
                            MotorType.Synchronous => 3,
                        MotorType.WoundRotor => 1.5,
                        _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
                    },
                CircuitProtection.DualElement =>
                    MotorType switch
                    {
                        MotorType.SinglePhaseMotor or
                            MotorType.SquirrelCage or
                            MotorType.DesignBEnergyEfficient or
                            MotorType.Synchronous => 1.75,
                        MotorType.WoundRotor => 1.5,
                        _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
                    },
                CircuitProtection.InstantaneousTripBreaker =>
                    MotorType switch
                    {
                        MotorType.DesignBEnergyEfficient => 11,
                        MotorType.SinglePhaseMotor or
                            MotorType.SquirrelCage or
                            MotorType.Synchronous or
                            MotorType.WoundRotor => 8,
                        _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
                    },
                CircuitProtection.InverseTimeBreaker =>
                    MotorType switch
                    {
                        MotorType.SinglePhaseMotor or
                            MotorType.SquirrelCage or
                            MotorType.DesignBEnergyEfficient or
                            MotorType.Synchronous => 2.5,
                        MotorType.WoundRotor => 1.5,
                        _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
                    },
                _ =>
                    throw new ArgumentOutOfRangeException(nameof(CircuitProtection))
            };

            var value = AmpereLoad * factor;
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
        LineToLineVoltage? lineToLineVoltage)
    {
        if (parentDistributionBoard.Phase == BoardPhase.SinglePhase || lineToLineVoltage != Enums.LineToLineVoltage.Abc)
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
}