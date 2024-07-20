using Scrumboard.Application.Abstractions.WeatherForecasts;
using Scrumboard.Domain.Weathers;

namespace Scrumboard.Application.WeatherForecasts;

internal sealed class WeatherForecastsService : IWeatherForecastsService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    public Task<IEnumerable<WeatherForecast>> GetAsync()
    {
        var random = new Random();

        var weatherForecasts = Enumerable.Range(1, 5)
            .Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = random.Next(-20, 55),
                Summary = Summaries[random.Next(Summaries.Length)]
            });
        
        return Task.FromResult(weatherForecasts);
    }
}
