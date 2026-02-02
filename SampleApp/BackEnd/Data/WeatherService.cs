using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data;

public interface IWeatherService
{
    Task<WeatherForecast[]> GetForecastAsync(DateTime? startDate);
}

public class WeatherService : IWeatherService
{
    private readonly WeatherDbContext _context;

    public WeatherService(WeatherDbContext context)
    {
        _context = context;
    }

    public async Task<WeatherForecast[]> GetForecastAsync(DateTime? startDate)
    {
        var query = _context.WeatherForecasts.AsQueryable();

        if (startDate.HasValue)
        {
            var startDateOnly = DateOnly.FromDateTime(startDate.Value);
            query = query.Where(f => f.Date >= startDateOnly);
        }

        return await query.OrderBy(f => f.Date).ToArrayAsync();
    }
}
