namespace automated_electrical_schedule.Data.Models;

public class Reference
{
    public string? Name { get; set; }

    public List<string> Descriptions { get; set; } = [];

    public Reference(string name)
    {
        Name = name;
    }

    public Reference(string name, List<string> descriptions)
    {
        Name = name;
        Descriptions = descriptions;
    }
}