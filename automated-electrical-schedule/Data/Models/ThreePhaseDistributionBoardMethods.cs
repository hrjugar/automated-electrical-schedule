using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class ThreePhaseDistributionBoard
{
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
            ConductorType = ConductorType,
            GroundingId = GroundingId,
            Grounding = Grounding,
            RacewayType = RacewayType,
            TransformerPrimaryProtection = TransformerPrimaryProtection,
            TransformerSecondaryProtection = TransformerSecondaryProtection,
            BreakerCircuitProtection = BreakerCircuitProtection,
            BreakerSetCount = BreakerSetCount,
            BreakerConductorTypeId = BreakerConductorTypeId,
            BreakerConductorType = BreakerConductorType,
            BreakerGroundingId = BreakerGroundingId,
            BreakerGrounding = BreakerGrounding,
            BreakerRacewayType = BreakerRacewayType,

            ThreePhaseConfiguration = ThreePhaseConfiguration
        };
    }

    public override List<BoardVoltage> GetAllowedVoltages()
    {
        List<BoardVoltage> phaseVoltages = ThreePhaseConfiguration switch
        {
            ThreePhaseConfiguration.Delta => [BoardVoltage.V230, BoardVoltage.V460, BoardVoltage.V575],
            ThreePhaseConfiguration.Wye => [BoardVoltage.V230, BoardVoltage.V400],
            _ => throw new ArgumentOutOfRangeException(nameof(ThreePhaseConfiguration))
        };

        var maxChildBoardVoltage = SubDistributionBoards.Count > 0
            ? (int)SubDistributionBoards.MaxBy(b => (int)b.Voltage)!.Voltage
            : 0;

        return ParentDistributionBoard == null
            ? phaseVoltages
            : phaseVoltages.Where(v => (int)v <= (int)ParentDistributionBoard.Voltage && (int)v >= maxChildBoardVoltage)
                .ToList();
    }

    public override List<LineToLineVoltage> GetAllowedLineToLineVoltages()
    {
        return [Enums.LineToLineVoltage.Abc];
    }

    public List<CircuitProtection> GetAllowedTransformerPrimaryProtection()
    {
        return ParentDistributionBoard == null ? [CircuitProtection.CutOutFuse] : AllowedCircuitProtections;
    }
}