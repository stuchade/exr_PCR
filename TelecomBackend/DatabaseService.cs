using System.Data;
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

    public static void DeleteLogs(int logId)
    {
        string sql = "DELETE FROM logs WHERE id = @Id;";
        using (var connection = GetConnection())
        {
            connection.Execute(sql, new {Id = logId});
        }
    }
} 