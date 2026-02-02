using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data;

public class WeatherDbContext : DbContext
{
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options) { }

    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure WeatherForecast entity
        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.TemperatureC).IsRequired();
            entity.Property(e => e.Summary).HasMaxLength(50);
        });

        // Seed initial data
        modelBuilder.Entity<WeatherForecast>().HasData(
            new WeatherForecast { Id = 1, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TemperatureC = 20, Summary = "Mild" },
            new WeatherForecast { Id = 2, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), TemperatureC = 25, Summary = "Warm" },
            new WeatherForecast { Id = 3, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(3)), TemperatureC = 15, Summary = "Cool" },
            new WeatherForecast { Id = 4, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(4)), TemperatureC = 10, Summary = "Chilly" },
            new WeatherForecast { Id = 5, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(5)), TemperatureC = 30, Summary = "Hot" }
        );
    }
}

public record WeatherForecast
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}
