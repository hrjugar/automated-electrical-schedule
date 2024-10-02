using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class MotorOutletCircuit
{
    public override Circuit Clone()
    {
        return new MotorOutletCircuit
        {
            Id = Id,
            ParentDistributionBoardId = ParentDistributionBoardId,
            ParentDistributionBoard = ParentDistributionBoard,
            CircuitType = CircuitType,
            Description = Description,
            Quantity = Quantity,
            WireLength = WireLength,
            DemandFactor = DemandFactor,
            CircuitProtection = CircuitProtection,
            SetCount = SetCount,
            ConductorTypeId = ConductorTypeId,
            ConductorType = ConductorType,
            GroundingId = GroundingId,
            Grounding = Grounding,
            RacewayType = RacewayType,

            MotorType = MotorType,
            Horsepower = Horsepower
        };
    }

    public override List<CircuitProtection> GetAllowedCircuitProtections()
    {
        return
        [
            CircuitProtection.NonTimeDelayFuse,
            CircuitProtection.DualElement,
            CircuitProtection.InstantaneousTripBreaker,
            CircuitProtection.InverseTimeBreaker
        ];
    }

    public override double GetVoltAmpere()
    {
        return GetVoltage() * GetAmpereLoad();
    }

    public override double GetAmpereLoad()
    {
        if (ParentDistributionBoard.Voltage == BoardVoltage.V230)
            return DataConstants.GetMotorOutlet230VoltAmpereLoad(Horsepower);

        // TODO: Update formula for other voltages
        return 1;
    }

    public override int GetAmpereTrip()
    {
        // TODO: Handle formula for fire pump

        var factor = CircuitProtection switch
        {
            CircuitProtection.NonTimeDelayFuse =>
                MotorType switch
                {
                    MotorType.SinglePhaseMotor or
                        MotorType.SquirrelCage or
                        MotorType.DesignBEnergyEfficient or
                        MotorType.Synchronous => 3,
                    MotorType.WoundRotor or
                        MotorType.DcConstantVoltage => 1.5,
                    _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
                },
            CircuitProtection.DualElement =>
                MotorType switch
                {
                    MotorType.SinglePhaseMotor or
                        MotorType.SquirrelCage or
                        MotorType.DesignBEnergyEfficient or
                        MotorType.Synchronous => 1.75,
                    MotorType.WoundRotor or
                        MotorType.DcConstantVoltage => 1.5,
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
                    MotorType.DcConstantVoltage => 2.5,
                    _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
                },
            CircuitProtection.InverseTimeBreaker =>
                MotorType switch
                {
                    MotorType.SinglePhaseMotor or
                        MotorType.SquirrelCage or
                        MotorType.DesignBEnergyEfficient or
                        MotorType.Synchronous => 2.5,
                    MotorType.WoundRotor or
                        MotorType.DcConstantVoltage => 1.5,
                    _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
                },
            _ =>
                throw new ArgumentOutOfRangeException(nameof(CircuitProtection))
        };

        var value = GetAmpereLoad() * factor;
        return DataConstants.GetAmpereTrip(value);
    }
}