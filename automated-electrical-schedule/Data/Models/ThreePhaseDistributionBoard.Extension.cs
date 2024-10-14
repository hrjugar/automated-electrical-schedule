using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.Models;

public partial class ThreePhaseDistributionBoard
{
    public override List<BoardVoltage> AllowedVoltages
    {
        get
        {
            List<BoardVoltage> phaseVoltages = ThreePhaseConfiguration switch
            {
                ThreePhaseConfiguration.Delta => [BoardVoltage.V230, BoardVoltage.V460, BoardVoltage.V575],
                ThreePhaseConfiguration.Wye => [BoardVoltage.V400],
                _ => throw new ArgumentOutOfRangeException(nameof(ThreePhaseConfiguration))
            };

            var maxChildBoardVoltage = SubDistributionBoards.Count > 0
                ? (int)SubDistributionBoards.MaxBy(b => (int)b.Voltage)!.Voltage
                : 0;

            return ParentDistributionBoard == null
                ? phaseVoltages
                : phaseVoltages.Where(v =>
                        (int)v <= (int)ParentDistributionBoard.Voltage && (int)v >= maxChildBoardVoltage)
                    .ToList();
        }
    }

    public override List<LineToLineVoltage> AllowedLineToLineVoltages => [Enums.LineToLineVoltage.Abc];

    protected override CalculationResult<double> Current
    {
        get
        {
            var highestPhaseAmpereLoad = new[] { AmpereLoadA, AmpereLoadB, AmpereLoadC }.Max();

            var totalAbcCircuitAmpereLoad = Circuits
                .Where(circuit => circuit.LineToLineVoltage == Enums.LineToLineVoltage.Abc)
                .Select(circuit => circuit.AmpereLoad)
                .Sum();

            var highestMotorLoadA = Circuits
                .OfType<MotorOutletCircuit>()
                .Where(circuit => circuit.LineToLineVoltage == Enums.LineToLineVoltage.A)
                .Select(circuit => circuit.AmpereLoad)
                .Max();

            var highestMotorLoadB = Circuits
                .OfType<MotorOutletCircuit>()
                .Where(circuit => circuit.LineToLineVoltage == Enums.LineToLineVoltage.B)
                .Select(circuit => circuit.AmpereLoad)
                .Max();

            var highestMotorLoadC = Circuits
                .OfType<MotorOutletCircuit>()
                .Where(circuit => circuit.LineToLineVoltage == Enums.LineToLineVoltage.C)
                .Select(circuit => circuit.AmpereLoad)
                .Max();

            var highestMotorLoadAbc = Circuits
                .OfType<MotorOutletCircuit>()
                .Where(circuit => circuit.LineToLineVoltage == Enums.LineToLineVoltage.Abc)
                .Select(circuit => circuit.AmpereLoad)
                .Max();

            var highestMotorLoad = new[]
            {
                highestMotorLoadA,
                highestMotorLoadB,
                highestMotorLoadC,
                highestMotorLoadAbc
            }.Max();

            var factor = ThreePhaseConfiguration == ThreePhaseConfiguration.Delta ? Math.Sqrt(3) : 1;
            var value = highestPhaseAmpereLoad * factor + totalAbcCircuitAmpereLoad + 0.25 * highestMotorLoad;
            return value == 0
                ? CalculationResult<double>.Failure(CalculationErrorType.NoBoardCurrent)
                : CalculationResult<double>.Success(value);
        }
    }

    public double AmpereLoadA => CalculateAmpereLoad(Enums.LineToLineVoltage.A);
    public double AmpereLoadB => CalculateAmpereLoad(Enums.LineToLineVoltage.B);
    public double AmpereLoadC => CalculateAmpereLoad(Enums.LineToLineVoltage.C);
    public double AmpereLoadAbc => CalculateAmpereLoad(Enums.LineToLineVoltage.Abc);

    public override double AmpereLoad
    {
        get
        {
            return LineToLineVoltage switch
            {
                Enums.LineToLineVoltage.A => AmpereLoadA,
                Enums.LineToLineVoltage.B => AmpereLoadB,
                Enums.LineToLineVoltage.C => AmpereLoadC,
                Enums.LineToLineVoltage.Abc => AmpereLoadA + AmpereLoadB + AmpereLoadC + AmpereLoadAbc,
                _ => throw new ArgumentOutOfRangeException(nameof(LineToLineVoltage))
            };
        }
    }

    public override DistributionBoard Clone()
    {
        return new ThreePhaseDistributionBoard
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
            BreakerRacewayType = BreakerRacewayType,

            ThreePhaseConfiguration = ThreePhaseConfiguration
        };
    }

    private double CalculateAmpereLoad(LineToLineVoltage lineToLineVoltage)
    {
        double totalAmpereLoad = 0;

        var childCircuitsAmpereLoad = Circuits
            .Where(circuit => circuit.LineToLineVoltage == lineToLineVoltage)
            .Select(circuit => circuit.AmpereLoad)
            .Sum();

        totalAmpereLoad += childCircuitsAmpereLoad;

        // if (lineToLineVoltage == Enums.LineToLineVoltage.Abc) return totalAmpereLoad;

        foreach (var subBoard in SubDistributionBoards)
            switch (subBoard)
            {
                case ThreePhaseDistributionBoard subThreePhaseBoard:
                    totalAmpereLoad += subThreePhaseBoard.CalculateAmpereLoad(lineToLineVoltage);
                    break;
                case SinglePhaseDistributionBoard subSinglePhaseBoard:
                    if (subSinglePhaseBoard.LineToLineVoltage == lineToLineVoltage)
                        totalAmpereLoad += subSinglePhaseBoard.AmpereLoad;
                    break;
            }

        return totalAmpereLoad;
    }
}