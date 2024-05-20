using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace Scrumboard.Web.Api.WeatherForecast;

public class WeatherForecastController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Domain.Weathers.WeatherForecast>> Get() => 
        await Mediator.Send(new GetWeatherForecastsQuery());
}