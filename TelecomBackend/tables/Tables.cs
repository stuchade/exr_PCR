namespace TelecomBackend;

public class Logs
{
    public int Id {get; set;        }
    public string TripName {get; set; } = string.Empty;
    public DateTime CreatedAt {get; set; }
}

public class LogPoints
{
    public int Id {get; set;}
    public int TripId { get; set; }
    public DateTime MeasuredAtGps { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public double? Altitude {get; set;}
    public double? Speed { get; set; }
    public int? Mcc {get; set;}
    public int? Mnc {get; set;}
    public long? CellId {get; set;}
    public int? Dbm {get; set;}
    public int? Ta {get; set;}
    public double? Accuracy {get; set;}
    public double? Bearing {get; set;}
    public DateTime? MeasuredAtBts {get; set;}
    public string? NetType {get; set;} 
    
}