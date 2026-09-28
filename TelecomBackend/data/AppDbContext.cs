using Microsoft.EntityFrameworkCore;
using TelecomBackend.models;

namespace TelecomBackend.data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Log> Logs {get; set;}
    public DbSet<LogPoint> LogPoints {get; set;}
}