using BackEnd.Data;
using Xunit;

namespace BackEnd.Tests;

public class WeatherForecastEndpointTests
{
    [Fact]
    public void WeatherForecast_ValidatesTemperatureConversion()
    {
        // Arrange
        var forecast = new WeatherForecast
        {
            Id = 1,
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = 0,
            Summary = "Freezing"
        };

        // Act
        var fahrenheit = forecast.TemperatureF;

        // Assert
        Assert.True(fahrenheit > 0, "Fahrenheit value should be positive for 0°C");
    }

    [Theory]
    [InlineData(100)]
    [InlineData(0)]
    [InlineData(-40)]
    public void WeatherForecast_CalculatesFahrenheitCorrectly(int celsius)
    {
        // Arrange
        var forecast = new WeatherForecast
        {
            Id = 1,
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = celsius,
            Summary = "Test"
        };

        var expected = 32 + (int)(celsius / 0.5556);

        // Act
        var result = forecast.TemperatureF;

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void WeatherForecast_PropertiesAreAccessible()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Now);
        var forecast = new WeatherForecast
        {
            Id = 1,
            Date = today,
            TemperatureC = 20,
            Summary = "Mild"
        };

        // Act & Assert
        Assert.Equal(today, forecast.Date);
        Assert.Equal(20, forecast.TemperatureC);
        Assert.Equal("Mild", forecast.Summary);
    }
}
