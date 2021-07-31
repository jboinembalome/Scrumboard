using MediatR;
using Scrumboard.Domain.Entities;
using System.Collections.Generic;

namespace Scrumboard.Application.Features.WeathForecasts.Queries.GetWeatherForecasts
{
    public class GetWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecast>>
    {
    }
}
