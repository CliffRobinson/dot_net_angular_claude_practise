using Microsoft.AspNetCore.Mvc;

namespace DotNetApi.Features.WeatherForecasts;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private const int MinDays = 1;
    private const int MaxDays = 14;
    private const int DefaultDays = 5;
    private readonly IWeatherForecastService _weatherForecastService;
    public WeatherForecastController(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public ActionResult<WeatherForecast[]> Get()
    {
        return _weatherForecastService.GetWeatherForecast(DefaultDays);
    }

    [HttpGet("{days}")]
    public ActionResult<WeatherForecast[]> GetByDays(int days)
    {
        if (days < MinDays || days > MaxDays)
        {
            return BadRequest($"Need to request between {MinDays} and {MaxDays} days, got {days}");
        }
        return _weatherForecastService.GetWeatherForecast(days);
    }
}
