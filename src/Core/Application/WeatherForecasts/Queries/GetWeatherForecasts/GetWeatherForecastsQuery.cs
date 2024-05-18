using System.Collections.Generic;
using MediatR;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Application.WeatherForecasts.Queries.GetWeatherForecasts;

public sealed class GetWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecast>>
{
}