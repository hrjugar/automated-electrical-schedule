namespace automated_electrical_schedule.Data.Models;

public partial class Information
{
    public string Title { get; set; }
    
    public string? Subtitle { get; set; }
    
    public string? Description { get; set; }

    public List<Reference> References { get; set; } = [];

    public Information(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public Information(string title, List<Reference> references)
    {
        Title = title;
        References = references;
    }

    public Information(string title, string subtitle, string description)
    {
        Title = title;
        Subtitle = subtitle;
        Description = description;
    }
    
    public Information(string title, string subtitle, List<Reference> references)
    {
        Title = title;
        Subtitle = subtitle;
        References = references;
    }
}