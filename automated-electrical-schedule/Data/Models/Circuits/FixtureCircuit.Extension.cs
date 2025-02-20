using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Wrappers;

namespace automated_electrical_schedule.Data.Models;

public abstract partial class FixtureCircuit
{
    public override CalculationResult<double> VoltAmpere
    {
        get
        {
            var factor = LineToLineVoltage == LineToLineVoltage.Abc ? Math.Sqrt(3) : 1.0;
            return CalculationResult<double>.Success(Fixtures.Sum(fixture => fixture.Quantity * fixture.Wattage) * factor);
        }
    }
    
    public override CalculationResult<double> AmpereLoad => 
        CalculationResult<double>.Success(VoltAmpere.Value / Voltage);
}