namespace automated_electrical_schedule.Data.Models;

public partial record Information
{
    public string Title { get; set; }
    
    public string? Subtitle { get; set; }
    
    public string? Description { get; set; }

    public List<Reference> References { get; set; } = [];

    private Information(string title, string description)
    {
        Title = title;
        Description = description;
    }

    private Information(string title, List<Reference> references)
    {
        Title = title;
        References = references;
    }

    private Information(string title, string subtitle, string description)
    {
        Title = title;
        Subtitle = subtitle;
        Description = description;
    }
    
    private Information(string title, string subtitle, List<Reference> references)
    {
        Title = title;
        Subtitle = subtitle;
        References = references;
    }
}