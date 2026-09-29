using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Swashbuckle.AspNetCore.SwaggerUI;

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

var testData = new List<Logs>
{
    new Logs { TripName = "Testovací výlet", CreatedAt = DateTime.Now }
};
DatabaseService.InsertLogs(testData);
// DatabaseService.DeleteLogs(1);

app.Run();
