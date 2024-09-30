using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class MotorOutletCircuit
{
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
        // TODO: Update formula
        return 1;
    }

    public override double GetAmpereLoad()
    {
        // TODO: Update formula
        return 1;
    }

    public override double GetAmpereTrip()
    {
        // TODO: Handle formula for fire pump

        double factor;

        switch (CircuitProtection)
        {
            case CircuitProtection.NonTimeDelayFuse:
                factor = MotorType switch
                {
                    MotorType.SinglePhaseMotor or
                        MotorType.SquirrelCage or
                        MotorType.DesignBEnergyEfficient or
                        MotorType.Synchronous => 3,
                    MotorType.WoundRotor or
                        MotorType.DcConstantVoltage => 1.5,
                    _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
                };
                break;
            case CircuitProtection.DualElement:
                factor = MotorType switch
                {
                    MotorType.SinglePhaseMotor or
                        MotorType.SquirrelCage or
                        MotorType.DesignBEnergyEfficient or
                        MotorType.Synchronous => 1.75,
                    MotorType.WoundRotor or
                        MotorType.DcConstantVoltage => 1.5,
                    _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
                };
                break;
            case CircuitProtection.InstantaneousTripBreaker:
                factor = MotorType switch
                {
                    MotorType.DesignBEnergyEfficient => 11,
                    MotorType.SinglePhaseMotor or
                        MotorType.SquirrelCage or
                        MotorType.Synchronous or
                        MotorType.WoundRotor => 8,
                    MotorType.DcConstantVoltage => 2.5,
                    _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
                };
                break;
            case CircuitProtection.InverseTimeBreaker:
                factor = MotorType switch
                {
                    MotorType.SinglePhaseMotor or
                        MotorType.SquirrelCage or
                        MotorType.DesignBEnergyEfficient or
                        MotorType.Synchronous => 2.5,
                    MotorType.WoundRotor or
                        MotorType.DcConstantVoltage => 1.5,
                    _ => throw new ArgumentOutOfRangeException(nameof(MotorType))
                };
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(CircuitProtection));
        }

        if (ParentDistributionBoard.Phase == BoardPhase.SinglePhase)
            return factor;
        return GetAmpereLoad() * factor;
    }

    public override double GetAmpereFrame()
    {
        // TODO: Update formula
        return 1;
    }
}