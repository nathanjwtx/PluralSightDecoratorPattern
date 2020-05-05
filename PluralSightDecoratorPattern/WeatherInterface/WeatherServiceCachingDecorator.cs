using System;
using DecoratorDesignPattern.WeatherInterface;
using Microsoft.Extensions.Caching.Memory;

namespace PluralSightDecoratorPattern.WeatherInterface
{
    public class WeatherServiceCachingDecorator : IWeatherService
    {
        private readonly IWeatherService _innerWeatherService;
        private readonly IMemoryCache _cache;

        public WeatherServiceCachingDecorator(IWeatherService weatherService, IMemoryCache cache)
        {
            _innerWeatherService = weatherService;
            _cache = cache;
        }
        
        public CurrentWeather GetCurrentWeather(string location)
        {
            string cacheKey = $"WeatherConditions::{location}";
            
            // out var: if there is a value in the cache then it will return that value as currentWeather
            if (_cache.TryGetValue<CurrentWeather>(cacheKey, out var currentWeather))
            {
                return currentWeather;
            }
            else
            {
                var currentConditions = _innerWeatherService.GetCurrentWeather(location);
                _cache.Set<CurrentWeather>(cacheKey, currentConditions, TimeSpan.FromMinutes(30));
                return currentConditions;
            }
        }

        public LocationForecast GetForecast(string location)
        {
            string cacheKey = $"WeatherConditions::{location}";
            
            // out var: if there is a value in the cache then it will return that value as currentWeather
            if (_cache.TryGetValue<LocationForecast>(cacheKey, out var forecast))
            {
                return forecast;
            }
            else
            {
                var currentConditions = _innerWeatherService.GetForecast(location);
                _cache.Set<LocationForecast>(cacheKey, currentConditions, TimeSpan.FromMinutes(30));
                return currentConditions;
            }
        }
    }
}