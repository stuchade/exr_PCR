using Dapper;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

string connectionString = "Data Source=database.db";

var app = builder.Build();

app.UseCors("AllowAngular");

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    connection.Execute("PRAGMA foreign_keys = ON;");

    if (File.Exists("tables\\tables.sql"))
    {
        string sqlScript = File.ReadAllText("tables\\tables.sql");
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


app.Run();
