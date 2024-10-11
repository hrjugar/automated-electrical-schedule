using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class SinglePhaseDistributionBoard
{
    public override List<BoardVoltage> AllowedVoltages => [BoardVoltage.V230];

    public override List<LineToLineVoltage> AllowedLineToLineVoltages
    {
        get
        {
            if (ParentDistributionBoard is ThreePhaseDistributionBoard)
                return
                [
                    Enums.LineToLineVoltage.A,
                    Enums.LineToLineVoltage.B,
                    Enums.LineToLineVoltage.C
                ];

            return [];
        }
    }

    public override int AmpereTrip
    {
        get
        {
            double highestMotorLoad = 0;
            if (Circuits.Count > 0)
            {
                var motorWithHighestLoad =
                    Circuits.Where(c => c is MotorOutletCircuit).MaxBy(c => c.AmpereLoad);

                if (motorWithHighestLoad is MotorOutletCircuit) highestMotorLoad = motorWithHighestLoad.AmpereLoad;
            }

            var value = (AmpereLoad + 0.25 * highestMotorLoad) / 0.8;
            return DataUtils.GetAmpereTrip(value, 20);
        }
    }

    public override double AmpereLoad
    {
        get
        {
            var childCircuitsAmpereLoad = Circuits.Sum(circuit => circuit.AmpereLoad);
            var subBoardsAmpereLoad = SubDistributionBoards
                .OfType<SinglePhaseDistributionBoard>()
                .Sum(subBoard => subBoard.AmpereLoad);

            return childCircuitsAmpereLoad + subBoardsAmpereLoad;
        }
    }

    public override DistributionBoard Clone()
    {
        return new SinglePhaseDistributionBoard
        {
            Id = Id,
            BoardName = BoardName,
            ParentDistributionBoardId = ParentDistributionBoardId,
            ParentDistributionBoard = ParentDistributionBoard,
            Phase = Phase,
            Voltage = Voltage,
            WireLength = WireLength,
            CircuitProtection = CircuitProtection,
            LineToLineVoltage = LineToLineVoltage,
            SetCount = SetCount,
            ConductorTypeId = ConductorTypeId,
            GroundingId = GroundingId,
            RacewayType = RacewayType,
            TransformerPrimaryProtection = TransformerPrimaryProtection,
            TransformerSecondaryProtection = TransformerSecondaryProtection,
            BreakerCircuitProtection = BreakerCircuitProtection,
            BreakerSetCount = BreakerSetCount,
            BreakerConductorTypeId = BreakerConductorTypeId,
            BreakerGroundingId = BreakerGroundingId,
            BreakerRacewayType = BreakerRacewayType
        };
    }
}