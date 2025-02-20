using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Wrappers;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class FixtureCircuit
{
    public override CalculationResult<double> VoltAmpere =>
        CalculationResult<double>.Success(Fixtures.Sum(fixture => fixture.Quantity * fixture.Wattage * 1.0));

    public override CalculationResult<double> AmpereLoad
    {
        get
        {
            var factor = LineToLineVoltage == LineToLineVoltage.Abc ? Math.Sqrt(3) : 1;
            return CalculationResult<double>.Success(VoltAmpere.Value / (factor * Voltage));
        }
    }
}