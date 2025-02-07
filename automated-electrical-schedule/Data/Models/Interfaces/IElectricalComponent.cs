using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Wrappers;

namespace automated_electrical_schedule.Data.Models;

public interface IElectricalComponent
{
    public LineToLineVoltage LineToLineVoltage { get; set; }
    public CircuitProtection CircuitProtection { get; set; }
    public RacewayType RacewayType { get; set; }
    public string ConductorTypeId { get; set; }
    public string GroundingId { get; set; }
    public int SetCount { get; set; }
    public double WireLength { get; set; }
    
    public CalculationResult<double> AmpereLoad { get; }
    public CalculationResult<int> AmpereTrip { get; }
    public CalculationResult<int> AmpereFrame { get; }
    public CalculationResult<double> R { get; }
    public CalculationResult<double> X { get; }
    public CalculationResult<double> VoltageDrop { get; }
}