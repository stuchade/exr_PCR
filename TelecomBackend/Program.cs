using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Swashbuckle.AspNetCore.SwaggerUI;
using TelecomBackend;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = "Data Source=database.db";

var app = builder.Build();

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    connection.Execute("PRAGMA foreign_keys = ON;");

    if (File.Exists("tables.sql"))
    {
        string sqlScript = File.ReadAllText("tables.sql");
        connection.Execute(sqlScript);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// var testData = new List<Logs>
// {
//     new Logs { TripName = "Testovací výlet", CreatedAt = DateTime.Now }
// };
var id = DatabaseService.InsertLog("trasy\\trip1.gpx");
DatabaseService.InsertLogPoints("trasy\\trip1.csv", "trasy\\trip1.gpx", id);
// DatabaseService.DeleteLogs(1);
// List<CSVData> csvData = DataProcessing.GetCSVData("trasy\\trip1.csv");

// for (var i = 0; i < 10; i++)
// {
//     Console.WriteLine($"Cell id: {csvData[i].CellId}, Dbm: {csvData[i].Dbm}, Latitude: {csvData[i].Lat}, Longitude: {csvData[i].Lon}");
// }


app.Run();
