namespace DotNetApi.Features.WeatherForecasts;

public interface IWeatherForecastService
{
    WeatherForecast[] GetWeatherForecast(int days);
}
