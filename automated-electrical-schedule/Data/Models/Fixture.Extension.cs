namespace automated_electrical_schedule.Data.Models;

public partial class Fixture
{
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