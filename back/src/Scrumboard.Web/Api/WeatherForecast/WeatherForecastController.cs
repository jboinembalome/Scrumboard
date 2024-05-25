using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace Scrumboard.Web.Api.WeatherForecast;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class WeatherForecastController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Domain.Weathers.WeatherForecast>> Get() => 
        await mediator.Send(new GetWeatherForecastsQuery());
}
