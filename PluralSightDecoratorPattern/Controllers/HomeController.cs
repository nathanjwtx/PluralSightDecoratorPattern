using System;
using System.Diagnostics;
using DecoratorDesignPattern.Models;
using DecoratorDesignPattern.OpenWeatherMap;
using DecoratorDesignPattern.WeatherInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PluralSightDecoratorPattern.WeatherInterface;

namespace PluralSightDecoratorPattern.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherService _weatherService;

        public HomeController(ILogger<HomeController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }
        
        // non DI controller constrcutor
        // public HomeController(ILoggerFactory loggerFactory, IConfiguration configuration, IMemoryCache memoryCache)
        // {
        //     _loggerFactory = loggerFactory;
        //     _logger = _loggerFactory.CreateLogger<HomeController>();
        //
        //     String apiKey = configuration.GetValue<String>("AppSettings:OpenWeatherMapApiKey");
        //     // we could wrap the below decorator in the caching decorator but multiple wraps gets complicated
        //     // _weatherService = new WeatherServiceLoggingDecorator(
        //         // new WeatherService(apiKey), _loggerFactory.CreateLogger<WeatherServiceLoggingDecorator>());
        //     
        //     // chaining constructors is clearer than nesting
        //     IWeatherService concreteService = new WeatherService(apiKey);
        //     IWeatherService withLoggingDecorator = new WeatherServiceLoggingDecorator(concreteService, _loggerFactory.CreateLogger<WeatherServiceLoggingDecorator>());
        //     IWeatherService withCachingDecorator = new WeatherServiceCachingDecorator(withLoggingDecorator, memoryCache);
        //     _weatherService = withCachingDecorator;
        // }

        public IActionResult Index(string location = "Dallas")
        {
            CurrentWeather conditions = _weatherService.GetCurrentWeather(location);
            return View(conditions);
        }

        public IActionResult Forecast(string location = "Dallas")
        {
            LocationForecast forecast = _weatherService.GetForecast(location);
            return View(forecast);
        }

        public IActionResult ApiKey()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
