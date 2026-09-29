using DotNetApi.Features.WeatherForecasts;

using Microsoft.Extensions.Time.Testing;

namespace DotNetApi.Tests.Features.WeatherForecasts;

public class WeatherForecastServiceTests
{
    private readonly FakeTimeProvider _timeProvider = new(new DateTimeOffset(2026, 3, 31, 0, 0, 0, TimeSpan.Zero));

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(25)]

    public void GetWeatherForecast_ReturnsNForecasts_WhenCalledWithN(int days)
    {
        var sut = new WeatherForecastService(_timeProvider);

        WeatherForecast[] result = sut.GetWeatherForecast(days);

        Assert.IsType<WeatherForecast[]>(result);
        Assert.Equal(days, result.Length);
    }

    [Fact]
    public void GetWeatherForecast_ReturnsResultsWithinExpectedRanges_WhenCalled()
    {
        var sut = new WeatherForecastService(_timeProvider);

        WeatherForecast[] result = sut.GetWeatherForecast(5);

        Assert.IsType<WeatherForecast[]>(result);

        foreach (WeatherForecast weatherForecast in result)
        {
            Assert.InRange(weatherForecast.TemperatureC, WeatherForecastService.MinTemp, WeatherForecastService.MaxTemp);
            Assert.Contains(weatherForecast.Summary, WeatherForecastService.Summaries);
        }
    }

    [Fact]
    public void GetWeatherForecast_ReturnsConsecutiveDatesFromAprilFools_WhenCalledWithFakeTimer()
    {
        var sut = new WeatherForecastService(_timeProvider);
        var startDate = new DateOnly(2026, 4, 1);

        WeatherForecast[] result = sut.GetWeatherForecast(5);


        for (var i = 0; i < result.Length; i++)
        {
            Assert.Equal(startDate.AddDays(i), result[i].Date);
        }
    }
}
