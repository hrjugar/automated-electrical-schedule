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

    // protected override CalculationResult<double> Current
    // {
    //     get
    //     {
    //         var highestPhaseAmpereLoad = new[] { AmpereLoadA, AmpereLoadB, AmpereLoadC }.Max();
    //
    //         var totalAbcCircuitsAmpereLoad = Circuits
    //             .Where(circuit => circuit.LineToLineVoltage == Enums.LineToLineVoltage.Abc)
    //             .Select(circuit => circuit.AmpereLoad)
    //             .Sum();
    //
    //         var totalAbcSubBoardsAmpereLoad = SubDistributionBoards
    //             .OfType<ThreePhaseDistributionBoard>()
    //             .Select(board => board.AmpereLoadAbc)
    //             .Sum();
    //
    //         var totalAbcAmpereLoad = totalAbcCircuitsAmpereLoad + totalAbcSubBoardsAmpereLoad;
    //
    //         var highestMotorLoadA = Circuits
    //             .OfType<MotorOutletCircuit>()
    //             .Where(circuit => circuit.LineToLineVoltage == Enums.LineToLineVoltage.A)
    //             .Select(circuit => circuit.AmpereLoad)
    //             .Max();
    //
    //         var highestMotorLoadB = Circuits
    //             .OfType<MotorOutletCircuit>()
    //             .Where(circuit => circuit.LineToLineVoltage == Enums.LineToLineVoltage.B)
    //             .Select(circuit => circuit.AmpereLoad)
    //             .Max();
    //
    //         var highestMotorLoadC = Circuits
    //             .OfType<MotorOutletCircuit>()
    //             .Where(circuit => circuit.LineToLineVoltage == Enums.LineToLineVoltage.C)
    //             .Select(circuit => circuit.AmpereLoad)
    //             .Max();
    //
    //         var highestMotorLoadAbc = Circuits
    //             .OfType<MotorOutletCircuit>()
    //             .Where(circuit => circuit.LineToLineVoltage == Enums.LineToLineVoltage.Abc)
    //             .Select(circuit => circuit.AmpereLoad)
    //             .Max();
    //
    //         var highestMotorLoad = new[]
    //         {
    //             highestMotorLoadA,
    //             highestMotorLoadB,
    //             highestMotorLoadC,
    //             highestMotorLoadAbc
    //         }.Max();
    //
    //         var factor = ThreePhaseConfiguration == ThreePhaseConfiguration.Delta ? Math.Sqrt(3) : 1;
    //         var value = highestPhaseAmpereLoad * factor + totalAbcAmpereLoad + 0.25 * highestMotorLoad;
    //         return value == 0
    //             ? CalculationResult<double>.Failure(CalculationErrorType.NoBoardCurrent)
    //             : CalculationResult<double>.Success(value);
    //     }
    // }

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

    public void BalanceLoads()
    {
        var toBeBalancedCircuits = Circuits
            .Where(circuit =>
                (circuit.LineToLineVoltage == LineToLineVoltage.A
                || circuit.LineToLineVoltage == LineToLineVoltage.B
                || circuit.LineToLineVoltage == LineToLineVoltage.C) &&
                circuit.Voltage == 230)
            .ToList();

        if (toBeBalancedCircuits.Count < 2) return;
        
        List<Circuit> cloneCircuits = [];
        foreach (var circuit in toBeBalancedCircuits)
        {
            cloneCircuits.Add(circuit.Clone());
        }
        
        List<LineToLineVoltage> possibleLineToLineVoltages = [ 
            LineToLineVoltage.A, 
            LineToLineVoltage.B, 
            LineToLineVoltage.C 
        ];
        
        var subBoardLineToLineVoltageLoads = new Dictionary<LineToLineVoltage, double>();
        
        foreach (var lineToLineVoltage in possibleLineToLineVoltages)
        {
            var singlePhaseSubBoardLineToLineVoltageLoad = SubDistributionBoards
                .OfType<SinglePhaseDistributionBoard>()
                .Where(subBoard => subBoard.LineToLineVoltage == lineToLineVoltage)
                .Select(subBoard => subBoard.AmpereLoad)
                .Sum();
            
            var threePhaseSubBoardLineToLineVoltageLoad = SubDistributionBoards
                .OfType<ThreePhaseDistributionBoard>()
                .Select(subBoard => lineToLineVoltage switch
                {
                    LineToLineVoltage.A => subBoard.AmpereLoadA,
                    LineToLineVoltage.B => subBoard.AmpereLoadB,
                    LineToLineVoltage.C => subBoard.AmpereLoadC,
                    LineToLineVoltage.Abc or _ => 0
                })
                .Sum();
        
            subBoardLineToLineVoltageLoads[lineToLineVoltage] =
                singlePhaseSubBoardLineToLineVoltageLoad + threePhaseSubBoardLineToLineVoltageLoad;
        }

        var bestStandardDeviation = double.MaxValue;

        foreach (var combination in GetCombinations(cloneCircuits.Count, possibleLineToLineVoltages))
        {
            for (var i = 0; i < cloneCircuits.Count; i++)
            {
                cloneCircuits[i].LineToLineVoltage = combination[i];
            }

            var totalA = 
                cloneCircuits
                    .Where(c => c.LineToLineVoltage == LineToLineVoltage.A)
                    .Select(c => c.AmpereLoad)
                    .Sum() + 
                subBoardLineToLineVoltageLoads[LineToLineVoltage.A];
            
            var totalB =
                cloneCircuits
                    .Where(c => c.LineToLineVoltage == LineToLineVoltage.B)
                    .Select(c => c.AmpereLoad)
                    .Sum() + 
                subBoardLineToLineVoltageLoads[LineToLineVoltage.B];
            
            var totalC =
                cloneCircuits
                    .Where(c => c.LineToLineVoltage == LineToLineVoltage.C)
                    .Select(c => c.AmpereLoad)
                    .Sum() + 
                subBoardLineToLineVoltageLoads[LineToLineVoltage.C];

            List<double> totals = [totalA, totalB, totalC];
            var currentStandardDeviation = DataUtils.CalculateStandardDeviation(totals);

            if (currentStandardDeviation < bestStandardDeviation)
            {
                bestStandardDeviation = currentStandardDeviation;

                for (var i = 0; i < toBeBalancedCircuits.Count; i++)
                {
                    toBeBalancedCircuits[i].LineToLineVoltage = cloneCircuits[i].LineToLineVoltage;
                }
            }
        }

        return;
        
        IEnumerable<List<LineToLineVoltage>> GetCombinations(int count, List<LineToLineVoltage> lineToLineVoltages)
        {
            if (count == 0)
            {
                yield return [];
            }
            else
            {
                foreach (var lineToLineVoltage in lineToLineVoltages)
                {
                    foreach (var combination in GetCombinations(count - 1, lineToLineVoltages))
                    {
                        combination.Insert(0, lineToLineVoltage);
                        yield return combination;
                    }
                }
            }
        }
    }

    // public void BalanceLoads()
    // {
    //     var toBeBalancedCircuits = Circuits
    //         .Where(circuit =>
    //             (circuit.LineToLineVoltage == LineToLineVoltage.A
    //             || circuit.LineToLineVoltage == LineToLineVoltage.B
    //             || circuit.LineToLineVoltage == LineToLineVoltage.C) &&
    //             circuit.Voltage == 230)
    //         .OrderByDescending(circuit => circuit.AmpereLoad.Value)
    //         .ToList();
    //     
    //     if (toBeBalancedCircuits.Count < 2) return;
    //     
    //     var possibleLineToLineVoltages = new[] { LineToLineVoltage.A, LineToLineVoltage.B, LineToLineVoltage.C};
    //
    //     var lineToLineVoltageLoads = new Dictionary<LineToLineVoltage, double>();
    //     
    //     foreach (var lineToLineVoltage in possibleLineToLineVoltages)
    //     {
    //         lineToLineVoltageLoads[lineToLineVoltage] = 0;
    //     }
    //
    //     var subBoardLineToLineVoltageLoads = new Dictionary<LineToLineVoltage, double>();
    //
    //     foreach (var lineToLineVoltage in possibleLineToLineVoltages)
    //     {
    //         var singlePhaseSubBoardLineToLineVoltageLoad = SubDistributionBoards
    //             .OfType<SinglePhaseDistributionBoard>()
    //             .Where(subBoard => subBoard.LineToLineVoltage == lineToLineVoltage)
    //             .Select(subBoard => subBoard.AmpereLoad)
    //             .Sum();
    //         
    //         var threePhaseSubBoardLineToLineVoltageLoad = SubDistributionBoards
    //             .OfType<ThreePhaseDistributionBoard>()
    //             .Select(subBoard => lineToLineVoltage switch
    //             {
    //                 LineToLineVoltage.A => subBoard.AmpereLoadA,
    //                 LineToLineVoltage.B => subBoard.AmpereLoadB,
    //                 LineToLineVoltage.C => subBoard.AmpereLoadC,
    //                 LineToLineVoltage.Abc => subBoard.AmpereLoad,
    //                 _ => 0
    //             })
    //             .Sum();
    //
    //         subBoardLineToLineVoltageLoads[lineToLineVoltage] =
    //             singlePhaseSubBoardLineToLineVoltageLoad + threePhaseSubBoardLineToLineVoltageLoad;
    //     }
    //     
    //     UpdateLineToLineVoltageLoads();
    //     var currentStandingDeviation = CalculateStandardDeviation();
    //
    //     foreach (var circuit in toBeBalancedCircuits)
    //     {
    //         var originalLineToLineVoltage = circuit.LineToLineVoltage;
    //         var bestLineToLineVoltage = originalLineToLineVoltage;
    //         var bestStandingDeviation = currentStandingDeviation;
    //         
    //         foreach (var lineToLineVoltage in possibleLineToLineVoltages)
    //         {
    //             if (lineToLineVoltage == originalLineToLineVoltage) continue;
    //             
    //             circuit.LineToLineVoltage = lineToLineVoltage;
    //             UpdateLineToLineVoltageLoads();
    //             var newStandingDeviation = CalculateStandardDeviation();
    //             
    //             if (newStandingDeviation < bestStandingDeviation)
    //             {
    //                 bestStandingDeviation = newStandingDeviation;
    //                 bestLineToLineVoltage = lineToLineVoltage;
    //             }
    //             
    //             circuit.LineToLineVoltage = originalLineToLineVoltage;
    //             UpdateLineToLineVoltageLoads();
    //         }
    //         
    //         if (bestLineToLineVoltage != originalLineToLineVoltage)
    //         {
    //             circuit.LineToLineVoltage = bestLineToLineVoltage;
    //             currentStandingDeviation = bestStandingDeviation;
    //             UpdateLineToLineVoltageLoads();
    //         }
    //     }
    //
    //     return;
    //     
    //     void UpdateLineToLineVoltageLoads()
    //     {
    //         foreach (var lineToLineVoltage in possibleLineToLineVoltages)
    //         {
    //             var circuitsLoad =
    //                 toBeBalancedCircuits
    //                     .Where(circuit => circuit.LineToLineVoltage == lineToLineVoltage)
    //                     .Select(circuit => circuit.AmpereLoad)
    //                     .Sum();
    //
    //             lineToLineVoltageLoads[lineToLineVoltage] =
    //                 subBoardLineToLineVoltageLoads[lineToLineVoltage] + circuitsLoad;
    //         }
    //     }
    //     
    //     double CalculateStandardDeviation()
    //     {
    //         var mean = lineToLineVoltageLoads.Values.Average();
    //         var differenceSquaresSum = lineToLineVoltageLoads.Values.Sum(load => Math.Pow(load - mean, 2));
    //         return Math.Sqrt(differenceSquaresSum / lineToLineVoltageLoads.Count);
    //     }
    // }
}