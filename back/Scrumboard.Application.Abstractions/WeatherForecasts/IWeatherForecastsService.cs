using Scrumboard.Domain.Weathers;

namespace Scrumboard.Application.Abstractions.WeatherForecasts;

public interface IWeatherForecastsService
{
    Task<IEnumerable<WeatherForecast>> GetAsync();
}
