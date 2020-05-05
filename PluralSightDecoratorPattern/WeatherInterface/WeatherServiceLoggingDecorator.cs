using System.Diagnostics;
using DecoratorDesignPattern.WeatherInterface;
using Microsoft.Extensions.Logging;

namespace PluralSightDecoratorPattern.WeatherInterface
{
    public class WeatherServiceLoggingDecorator : IWeatherService
    {
        private readonly ILogger<WeatherServiceLoggingDecorator> _logger;
        private IWeatherService _innerWeatherService;

        // must provide a default implementation for every method in the interface or base class for the decorator pattern to work
        
        public WeatherServiceLoggingDecorator(IWeatherService weatherService, ILogger<WeatherServiceLoggingDecorator> logger)
        {
            _innerWeatherService = weatherService;
            _logger = logger;
        }
        public CurrentWeather GetCurrentWeather(string location)
        {
            // we are decorating the _innerWeatherService with calls before and after it
            Stopwatch sw = Stopwatch.StartNew();
            CurrentWeather currentWeather = _innerWeatherService.GetCurrentWeather(location);
            sw.Stop();
            long elapsedMillis = sw.ElapsedMilliseconds;
            _logger.LogWarning($"Retrieved weather data for {location} - Elapsed ms: {elapsedMillis} {currentWeather}");

            return currentWeather;
        }

        public LocationForecast GetForecast(string location)
        {
            Stopwatch sw = Stopwatch.StartNew();
            LocationForecast forecast = _innerWeatherService.GetForecast(location);
            sw.Stop();
            long elapsedMillis = sw.ElapsedMilliseconds;
            _logger.LogWarning($"Retrieved weather data for {location} - Elapsed ms: {elapsedMillis} {forecast}");

            return forecast;
        }
    }
}