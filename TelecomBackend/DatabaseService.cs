using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

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

    public static void InsertLogs(List<Logs> logs)
    {
        string sql = "INSERT INTO logs (trip_name, created_at) VALUES (@TripName, @CreatedAt);";
        using (var connection = GetConnection())
        {
            connection.Execute(sql, logs);
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