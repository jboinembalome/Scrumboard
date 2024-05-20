using MediatR;
using Scrumboard.Domain.Weathers;

namespace Scrumboard.Application.WeatherForecasts.Queries.GetWeatherForecasts;

public sealed class GetWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecast>>
{
}