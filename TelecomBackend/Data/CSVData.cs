using CsvHelper.Configuration.Attributes;

namespace TelecomBackend;
public class CSVData
{
    [Name("mcc")]
    public int Mcc {get; set;} // Mobile Country Code
    [Name("mnc")]
    public int Mnc {get; set;} // Mobile Network Code
    [Name("cell_id")]
    public long CellId {get; set;}
    [Name("dbm")]
    public int? Dbm {get; set;}
    [Name("ta")]
    public int? Ta {get; set;} // Timing Advance
    [Name("lat")]
    public double Lat {get; set;} // Latitude
    [Name("lon")]
    public double Lon {get; set;} // Longitude
    [Name("accuracy")]
    public double? Accuracy {get; set;}
    [Name("speed")]
    public double? Speed {get; set;}
    [Name("bearing")]
    public double? Bearing {get; set;} // Direction of movement in degrees
    [Name("altitude")]
    public double? Altitude {get; set;}
    [Name("measured_at")]
    public DateTime MeasuredAt {get; set;}
    [Name("net_type")]
    public string NetType {get; set;} = string.Empty;
}