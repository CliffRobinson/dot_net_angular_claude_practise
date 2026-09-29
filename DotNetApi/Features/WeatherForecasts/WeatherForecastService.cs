namespace DotNetApi.Features.WeatherForecasts;

public class WeatherForecastService(TimeProvider timeProvider) : IWeatherForecastService
{
    internal static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];
    internal const int MinTemp = -20;
    internal const int MaxTemp = 55;

    public WeatherForecast[] GetWeatherForecast(int days)
    {
        return Enumerable.Range(1, days).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime.AddDays(index)),
            TemperatureC = Random.Shared.Next(MinTemp, MaxTemp),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
