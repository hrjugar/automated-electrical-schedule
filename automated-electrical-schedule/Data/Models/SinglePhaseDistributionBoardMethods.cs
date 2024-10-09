using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public partial class SinglePhaseDistributionBoard
{
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
            BreakerRacewayType = BreakerRacewayType
        };
    }

    public override List<BoardVoltage> GetAllowedVoltages()
    {
        return [BoardVoltage.V230];
    }

    public override List<LineToLineVoltage> GetAllowedLineToLineVoltages()
    {
        if (ParentDistributionBoard is ThreePhaseDistributionBoard)
            return [Enums.LineToLineVoltage.Ab, Enums.LineToLineVoltage.Bc, Enums.LineToLineVoltage.Ca];

        return [];
    }
}