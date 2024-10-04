using automated_electrical_schedule.Data.Enums;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class DistributionBoard
{
    public abstract DistributionBoard Clone();

    public abstract List<BoardVoltage> GetAllowedVoltages();
}