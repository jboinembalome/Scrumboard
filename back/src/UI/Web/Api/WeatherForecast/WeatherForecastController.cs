using Microsoft.AspNetCore.Mvc;
using Scrumboard.Domain.Entities;
using Scrumboard.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace Scrumboard.Web.Controllers;

public class WeatherForecastController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get() => 
        await Mediator.Send(new GetWeatherForecastsQuery());
}