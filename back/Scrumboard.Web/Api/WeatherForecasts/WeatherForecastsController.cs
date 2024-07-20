using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrumboard.Application.Abstractions.WeatherForecasts;
using Scrumboard.Domain.Weathers;

namespace Scrumboard.Web.Api.WeatherForecasts;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class WeatherForecastsController(
    IWeatherForecastsService weatherForecastsService) : ControllerBase
{
    [HttpGet]
    public Task<IEnumerable<WeatherForecast>> Get() 
        => weatherForecastsService.GetAsync();
}
