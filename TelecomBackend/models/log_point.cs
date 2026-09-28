namespace TelecomBackend.models;

public class LogPoint
{
    public int Id {get; set;}
    public int TripId {get; set;}
    public DateTime MeasuredAt {get; set;}
    public double Lat {get; set;}
    public double Lon {get; set;}
    public int ShortCellId {get; set;}
    public int SignalStrenght {get; set;}
    public double? Speed {get; set;}
    public string? NetType {get; set;}

    public Log? Trip {get; set;}

}