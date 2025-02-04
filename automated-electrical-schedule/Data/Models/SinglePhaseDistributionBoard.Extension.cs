using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Extensions;

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

    // protected override CalculationResult<double> Current
    // {
    //     get
    //     {
    //         double highestMotorLoad = 0;
    //         if (Circuits.Count > 0)
    //         {
    //             highestMotorLoad =
    //                 Circuits.OfType<MotorOutletCircuit>().Select(c => c.AmpereLoad).Max();
    //         }
    //
    //         return CalculationResult<double>.Success(AmpereLoad + 0.25 * highestMotorLoad);
    //     }
    // }

    public override double AmpereLoad
    {
        get
        {
            var childCircuitsAmpereLoad = Circuits.Select(circuit => circuit.AmpereLoad).Sum();
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