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
            CircuitProtection = CircuitProtection,
            SetCount = SetCount,
            ConductorTypeId = ConductorTypeId,
            ConductorType = ConductorType,
            GroundingId = GroundingId,
            Grounding = Grounding,
            RacewayType = RacewayType,

            ThreePhaseConfiguration = ThreePhaseConfiguration,
            TransformerPrimaryProtection = TransformerPrimaryProtection,
            TransformerSecondaryProtection = TransformerSecondaryProtection,
            LineToLineVoltage = LineToLineVoltage,
            BreakerCircuitProtection = BreakerCircuitProtection,
            BreakerSetCount = BreakerSetCount,
            BreakerConductorTypeId = BreakerConductorTypeId,
            BreakerConductorType = BreakerConductorType,
            BreakerGroundingId = BreakerGroundingId,
            BreakerGrounding = BreakerGrounding,
            BreakerRacewayType = BreakerRacewayType
        };
    }

    public List<LineToLineVoltage> GetAllowedLineToLineVoltages()
    {
        return ParentDistributionBoard switch
        {
            null => [LineToLineVoltage.Abc],
            ThreePhaseDistributionBoard threePhaseDistributionBoard => threePhaseDistributionBoard.LineToLineVoltage
                switch
                {
                    LineToLineVoltage.Ab => [LineToLineVoltage.Ab],
                    LineToLineVoltage.Bc => [LineToLineVoltage.Bc],
                    LineToLineVoltage.Ca => [LineToLineVoltage.Ca],
                    LineToLineVoltage.Abc =>
                        [LineToLineVoltage.Ab, LineToLineVoltage.Bc, LineToLineVoltage.Ca, LineToLineVoltage.Abc],
                    _ => throw new ArgumentOutOfRangeException(nameof(LineToLineVoltage))
                },
            _ => throw new ArgumentOutOfRangeException(nameof(ParentDistributionBoard))
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

        return ParentDistributionBoard == null
            ? phaseVoltages
            : phaseVoltages.Where(v => (int)v <= (int)ParentDistributionBoard.Voltage).ToList();
    }

    public List<CircuitProtection> GetAllowedTransformerPrimaryProtection()
    {
        return ParentDistributionBoard == null ? [CircuitProtection.CutOutFuse] : AllowedCircuitProtections;
    }
}