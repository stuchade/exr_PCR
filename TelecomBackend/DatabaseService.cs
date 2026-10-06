using Dapper;
using Microsoft.Data.Sqlite;

namespace TelecomBackend;

public static class DatabaseService
{
    private static string connectionString = "Data Source=database.db";

    public static SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(connectionString);
        
        connection.Open();
        connection.Execute("PRAGMA foreign_keys = ON;");
        return connection;
    }

    public static int InsertLog(string gpxPath)
    {
        Logs log = DataProcessing.GetLogsData(gpxPath);
        string sql = @"INSERT INTO logs (trip_name, created_at) VALUES (@TripName, @CreatedAt);
                        SELECT last_insert_rowid();";
        using (var connection = GetConnection())
        {
            return connection.QuerySingle<int>(sql, log);
        }
    }

    public static List<Logs> GetLogs()
    {
        string sql = "SELECT id AS Id, trip_name AS TripName, created_at AS CreatedAt FROM logs ORDER BY created_at DESC;";
        using (var connection = GetConnection())
        {
            return connection.Query<Logs>(sql).ToList();
        }
    }

    public static void InsertLogPoints(string csvPath, string gpxPath, int logId)
    {
        List<LogPoints> logpoints = DataProcessing.GetLogPointsData(csvPath, gpxPath, logId);
        string sql = @"INSERT INTO log_points (trip_id, measured_at_gps, lat, lon, altitude, speed, mcc, mnc, cell_id, dbm, ta, accuracy, bearing, measured_at_bts, net_type) 
                    VALUES (@TripId, @MeasuredAtGps, @Lat, @Lon, @Altitude, @Speed, @Mcc, @Mnc, @CellId, @Dbm, @Ta, @Accuracy, @Bearing, @MeasuredAtBts, @NetType)";
        using (var connection = GetConnection())
        {
            using (var trasaction = connection.BeginTransaction())
            {
                connection.Execute(sql, logpoints, transaction: trasaction);
                trasaction.Commit();
            }
        }
    }

    public static List<LogPoints> GetLogPoints(int tripId)
    {
        string sql = @"SELECT trip_id AS TripId, measured_at_gps AS MeasuredAtGps, lat AS Lat, 
            lon AS Lon, altitude AS Altitude, speed AS Speed, mcc AS Mcc, mnc AS Mnc, cell_id AS CellId, 
            dbm AS Dbm, ta AS Ta, accuracy AS Accuracy, bearing AS Bearing, measured_at_bts AS MeasuredAtBts, 
            net_type AS NetType FROM log_points WHERE trip_id = @TripId;";

        using (var connection = GetConnection())
        {
            return connection.Query<LogPoints>(sql, new {TripId = tripId}).ToList();
        }
    }

    public static int InsertTrip(string csvPath, string gpxPath)
    {
        var id = InsertLog(gpxPath);
        InsertLogPoints(csvPath, gpxPath, id);
        return id;
    }

    public static void DeleteLogs(int logId)
    {
        string sql = "DELETE FROM logs WHERE id = @Id;";
        using (var connection = GetConnection())
        {
            connection.Execute(sql, new {Id = logId});
        }
    }
} 