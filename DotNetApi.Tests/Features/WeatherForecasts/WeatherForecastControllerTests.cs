using DotNetApi.Features.WeatherForecasts;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;

namespace DotNetApi.Tests.Features.WeatherForecasts;

public class WeatherForecastControllerTests
{
    private readonly IWeatherForecastService _weatherForecastService = Substitute.For<IWeatherForecastService>();
    private readonly WeatherForecastController _sut;

    public WeatherForecastControllerTests()
    {
        _sut = new WeatherForecastController(_weatherForecastService);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(15)]
    public void GetByDays_ReturnsBadRequest_WhenDaysIsOutsideRange(int days)
    {
        ActionResult<WeatherForecast[]> result = _sut.GetByDays(days);

        Assert.IsType<BadRequestObjectResult>(result.Result);
        _weatherForecastService.DidNotReceive().GetWeatherForecast(Arg.Any<int>());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(14)]
    public void GetByDays_CallsServiceWithDays_WhenDaysWithinRange(int days)
    {
        ActionResult<WeatherForecast[]> result = _sut.GetByDays(days);

        Assert.Null(result.Result);
        _weatherForecastService.Received(1).GetWeatherForecast(days);
    }
}
