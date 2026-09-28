namespace TelecomBackend.models;

public class Log
{
    public int Id {get; set;}
    public string TripName {get; set;} = string.Empty;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public List<LogPoint> LogPoints {get; set;} = new();
}