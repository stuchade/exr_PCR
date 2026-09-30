using System.Data;
using System.Globalization;
using System.Xml.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using TelecomBackend;


public static class DataProcessing
{
    public static List<CSVData> GetCSVData (string csvPath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        };

        using (var fileStream = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var reader = new StreamReader(fileStream))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csv.GetRecords<CSVData>().ToList();
        }
    }

    public static List<GPXData> GetGPXData (string gpxPath)
    {
        XDocument gpxDoc = XDocument.Load(gpxPath);
        XNamespace gpx = XNamespace.Get("http://www.topografix.com/GPX/1/0");


        var track = gpxDoc.Descendants(gpx + "trk").FirstOrDefault();
        
        if (track == null)
        {
            return new List<GPXData>();
        }

        var trackpoints = from trackpoint in track.Descendants(gpx + "trkpt")
            select new
            {
                Latitude = trackpoint.Attribute("lat")?.Value,
                Longitude = trackpoint.Attribute("lon")?.Value,
                Elevation = trackpoint.Element(gpx + "ele")?.Value,
                Time = trackpoint.Element(gpx + "time")?.Value,
                Speed = trackpoint.Element(gpx + "speed")?.Value
            };
                
          
        List<GPXData> gpxDatas = new List<GPXData>();
            
        foreach (var trkpt in trackpoints)
        {
            GPXData gpxData = new GPXData();
            gpxData.Lat = double.Parse(trkpt.Latitude, System.Globalization.CultureInfo.InvariantCulture);
            gpxData.Lon = double.Parse(trkpt.Longitude, System.Globalization.CultureInfo.InvariantCulture);

            if (double.TryParse(trkpt.Elevation, System.Globalization.CultureInfo.InvariantCulture, out double ele))
                gpxData.Ele = ele;

            if (DateTime.TryParse(trkpt.Time, out DateTime time))
                gpxData.Time = time;

            if (double.TryParse(trkpt.Speed, System.Globalization.CultureInfo.InvariantCulture, out double speed))
                gpxData.Speed = speed;

            gpxDatas.Add(gpxData);
        }
        
        return gpxDatas;
    }

    public static Logs GetLogsData(string gpxPath)
    {
        XDocument gpxDoc = XDocument.Load(gpxPath);
        XNamespace gpx = XNamespace.Get("http://www.topografix.com/GPX/1/0");

        string? timeText = gpxDoc.Root?.Element(gpx + "time")?.Value;
        DateTime gpxTime = DateTime.TryParse(timeText, out DateTime parsed) ? parsed : DateTime.Now;
        string gpxName = gpxDoc.Root?.Element(gpx + "name")?.Value ?? "Trip_" + timeText;

        // List<Logs> logs = new List<Logs>();
        Logs log = new Logs
        {
            TripName = gpxName,
            CreatedAt = gpxTime
        };
        // logs.Add(log);
        return log;
    }

    public static List<LogPoints> GetLogPointsData(string csvPath, string gpxPath, int logId)
    {
        List<CSVData> csvData = GetCSVData(csvPath);
        List<GPXData> gpxData = GetGPXData(gpxPath);

        List<LogPoints> logPointsList = new List<LogPoints>();
        
        if (csvData.Count == 0 || gpxData.Count == 0)
        {
            return logPointsList;
        }
        
        var tmp = 0;
        foreach (var gpxPoint in gpxData)
        {
            LogPoints logpoint = new LogPoints();

            logpoint.TripId = logId;
            logpoint.MeasuredAtGps = gpxPoint.Time;
            logpoint.Lat = gpxPoint.Lat;
            logpoint.Lon = gpxPoint.Lon;
            logpoint.Altitude = gpxPoint.Ele;
            logpoint.Speed = gpxPoint.Speed;

            
           
            while (tmp < csvData.Count - 1 &&
                    gpxPoint.Time > csvData[tmp + 1].MeasuredAt)
            {
                tmp++; 
                
            }
            var csvPoint = csvData[tmp];

            logpoint.Mcc = csvPoint.Mcc;
            logpoint.Mnc = csvPoint.Mnc;
            logpoint.CellId = csvPoint.CellId;
            logpoint.Dbm = csvPoint.Dbm;
            logpoint.Ta = csvPoint.Ta;
            logpoint.Accuracy = csvPoint.Accuracy;
            logpoint.Bearing = csvPoint.Bearing;
            logpoint.MeasuredAtBts = csvPoint.MeasuredAt;
            logpoint.NetType = csvPoint.NetType;

            logPointsList.Add(logpoint);
        }
        
        return logPointsList;
    }
}