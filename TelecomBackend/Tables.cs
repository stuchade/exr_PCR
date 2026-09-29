public class Logs
{
    public string TripName {get; set; } = string.Empty;
    public DateTime CreatedAt {get; set; }
}

public class LogPoints
{
    public int TripId { get; set; }
    public DateTime MeasuredAt { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public int? ShortCellId { get; set; }
    public int? SignalStrength { get; set; }
    public double? Speed { get; set; }
    public string? NetType { get; set; }
}