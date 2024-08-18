using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Utils;

namespace automated_electrical_schedule.Data.Models;

[Table(TableName)]
public class ConductorType
{
    private const string TableName = "conductor_types";

    public ConductorType()
    {
    }

    public ConductorType(ConductorMaterial material, ConductorTemperatureRating temperatureRating,
        ConductorWireType wireType)
    {
        Material = material;
        TemperatureRating = temperatureRating;
        WireType = wireType;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("material")] public ConductorMaterial Material { get; set; }

    [Column("temperature_rating")] public ConductorTemperatureRating TemperatureRating { get; set; }

    [Column("wire_type")] public ConductorWireType WireType { get; set; }

    public override string ToString()
    {
        return $"{WireType.GetDisplayName()} {Material.GetDisplayName()} ({TemperatureRating.GetDisplayName()})";
    }
}