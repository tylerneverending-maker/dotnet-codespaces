using BackEnd.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BackEnd.Tests;

public class WeatherServiceTests
{
    private WeatherDbContext CreateTestDbContext()
    {
        var options = new DbContextOptionsBuilder<WeatherDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new WeatherDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }

    private void SeedTestData(WeatherDbContext context)
    {
        // Seed test data with unique IDs to avoid conflicts
        context.WeatherForecasts.AddRange(
            new WeatherForecast { Id = 101, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TemperatureC = 20, Summary = "Mild" },
            new WeatherForecast { Id = 102, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), TemperatureC = 25, Summary = "Warm" },
            new WeatherForecast { Id = 103, Date = DateOnly.FromDateTime(DateTime.Now.AddDays(3)), TemperatureC = 15, Summary = "Cool" }
        );
        context.SaveChanges();
    }

    [Fact]
    public async Task GetForecastAsync_WithoutStartDate_ReturnsAllForecasts()
    {
        // Arrange
        var context = CreateTestDbContext();
        SeedTestData(context);
        var service = new WeatherService(context);

        // Act
        var result = await service.GetForecastAsync(null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Length);
    }

    [Fact]
    public async Task GetForecastAsync_WithStartDate_ReturnsFilteredForecasts()
    {
        // Arrange
        var context = CreateTestDbContext();
        SeedTestData(context);
        var service = new WeatherService(context);
        var today = DateTime.Now;
        var startDate = today.AddDays(1); // Start from day 1

        // Act
        var result = await service.GetForecastAsync(startDate);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, f => Assert.True(f.Date >= DateOnly.FromDateTime(startDate)));
    }

    [Fact]
    public async Task GetForecastAsync_ReturnsOrderedByDate()
    {
        // Arrange
        var context = CreateTestDbContext();
        SeedTestData(context);
        var service = new WeatherService(context);

        // Act
        var result = await service.GetForecastAsync(null);

        // Assert
        Assert.NotNull(result);
        for (int i = 1; i < result.Length; i++)
        {
            Assert.True(result[i].Date >= result[i - 1].Date, "Results should be ordered by date");
        }
    }

    [Fact]
    public async Task GetForecastAsync_ComputesTemperatureFahrenheit()
    {
        // Arrange
        var context = CreateTestDbContext();
        SeedTestData(context);
        var service = new WeatherService(context);

        // Act
        var result = await service.GetForecastAsync(null);

        // Assert
        var firstForecast = result.First();
        var expectedF = 32 + (int)(firstForecast.TemperatureC / 0.5556);
        Assert.Equal(expectedF, firstForecast.TemperatureF);
    }

    [Fact]
    public async Task GetForecastAsync_EmptyDatabase_ReturnsEmptyArray()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<WeatherDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new WeatherDbContext(options);
        var service = new WeatherService(context);

        // Act
        var result = await service.GetForecastAsync(null);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
