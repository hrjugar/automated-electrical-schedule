namespace automated_electrical_schedule.Data.Models;

public partial class SpaceCircuit
{
    public override Circuit Clone()
    {
        return new SpaceCircuit
        {
            Id = Id,
            ParentDistributionBoardId = ParentDistributionBoardId,
            ParentDistributionBoard = ParentDistributionBoard,
            Order = Order,
            CircuitType = CircuitType,
            LineToLineVoltage = LineToLineVoltage,
        };
    }
}