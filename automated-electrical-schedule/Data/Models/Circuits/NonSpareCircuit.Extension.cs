using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;
using automated_electrical_schedule.Data.Wrappers;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class NonSpareCircuit
{
    public abstract CalculationResult<double> VoltAmpere { get; }
}