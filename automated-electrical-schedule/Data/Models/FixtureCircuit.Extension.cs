namespace automated_electrical_schedule.Data.Models;

public abstract partial class FixtureCircuit
{
    public override CalculationResult<double> VoltAmpere => 
        CalculationResult<double>.Success(Fixtures.Sum(fixture => fixture.Quantity * fixture.Wattage / 0.85));
    
    public override CalculationResult<double> AmpereLoad => 
        CalculationResult<double>.Success(VoltAmpere.Value / Voltage);
}