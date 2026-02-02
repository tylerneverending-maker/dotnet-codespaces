using FrontEnd.Data;
using Xunit;

namespace FrontEnd.Tests;

public class WeatherForecastClientTests
{
    [Fact]
    public void WeatherForecast_PropertiesAreAccessible()
    {
        // Arrange
        var forecast = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = 20,
            Summary = "Mild"
        };

        // Act & Assert
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), forecast.Date);
        Assert.Equal(20, forecast.TemperatureC);
        Assert.Equal("Mild", forecast.Summary);
    }

    [Fact]
    public void WeatherForecast_ComputesTemperatureFahrenheit()
    {
        // Arrange
        var forecast = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = 20,
            Summary = "Mild"
        };

        var expected = 32 + (int)(20 / 0.5556);

        // Act
        var result = forecast.TemperatureF;

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(20)]
    [InlineData(-40)]
    public void WeatherForecast_TemperatureConversionIsAccurate(int celsius)
    {
        // Arrange
        var forecast = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = celsius,
            Summary = "Test"
        };

        var expected = 32 + (int)(celsius / 0.5556);

        // Act
        var result = forecast.TemperatureF;

        // Assert - Allow small rounding differences
        Assert.Equal(expected, result);
    }

    [Fact]
    public void WeatherForecast_AllowsNullSummary()
    {
        // Arrange & Act
        var forecast = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = 20,
            Summary = null
        };

        // Assert
        Assert.Null(forecast.Summary);
    }

    [Fact]
    public void WeatherForecast_DateOnlyIsCorrect()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Now);
        var forecast = new WeatherForecast
        {
            Date = today,
            TemperatureC = 20,
            Summary = "Mild"
        };

        // Act & Assert
        Assert.Equal(today, forecast.Date);
    }
}
