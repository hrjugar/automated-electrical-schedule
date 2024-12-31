namespace automated_electrical_schedule.Data.Models;

public partial class Fixture
{
    public override string ToString()
    {
        return $"FIXTURE {Id}: {Description} ({Quantity} @ {Wattage}W), PARENT CIRCUIT {ParentCircuitId}";
    }

    public Fixture Clone()
    {
        return new Fixture
        {
            Id = Id,
            ParentCircuitId = ParentCircuitId,
            ParentCircuit = ParentCircuit,
            Description = Description,
            Quantity = Quantity,
            Wattage = Wattage
        };
    }
}