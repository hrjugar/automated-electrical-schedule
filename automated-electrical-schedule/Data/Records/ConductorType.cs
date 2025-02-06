using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Extensions;

namespace automated_electrical_schedule.Data.Records;

public partial record ConductorType
{
    public ConductorType()
    {
    }

    public ConductorType(
        ConductorMaterial material, 
        ConductorTemperatureRating temperatureRating,
        ConductorWireType wireType)
    {
        Material = material;
        TemperatureRating = temperatureRating;
        WireType = wireType;
    }

    public ConductorMaterial Material { get; }

    public ConductorTemperatureRating TemperatureRating { get; }

    public ConductorWireType WireType { get; }

    public string Id => $"{WireType.GetDisplayName()}_{Material.GetDisplayName()}_{TemperatureRating.GetDisplayName()}";

    public override string ToString()
    {
        return $"{WireType.GetDisplayName()} {Material.GetDisplayName()} ({TemperatureRating.GetDisplayName()})";
    }
}