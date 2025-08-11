using ConfigurationExample.OptionModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly WeatherApiOptions _options;

        public HomeController(IConfiguration configuration,IOptions<WeatherApiOptions> options)
        {
            _configuration = configuration;
            _options = options.Value;
        }


        [Route("/")]
        public IActionResult Index()
        {
            // Accessing configuration values
            ViewBag.MyKey = _configuration["MyKey"];
            ViewBag.MyApiKey = _configuration.GetValue<string>("MyApiKey","The Default Key");
            ViewBag.ClientId = _configuration["weatherapi:ClientId"];
            ViewBag.ClientSecret = _configuration.GetValue("weatherapi:ClientSecret","Default");

            // Accessing a configuration section
            ViewBag.SectionClient = _configuration.GetSection("weatherapi")["ClientId"];

            // Accessing a configuration section as a strongly typed object
            WeatherApiOptions? options = _configuration.GetSection("weatherapi").Get<WeatherApiOptions>();
            ViewBag.OptionsClientId = options?.ClientID;
            ViewBag.OptionsClientSecret = options?.ClientSecret;

            // Binding a configuration section to a strongly typed object
            WeatherApiOptions weatherApiOptions = new WeatherApiOptions();
            _configuration.GetSection("weatherapi").Bind(weatherApiOptions);
            ViewBag.BoundClientId = weatherApiOptions.ClientID;

            // Accessing options via IOptions
            ViewBag.IOptionsClientId = _options.ClientID;

            return View();
        }
    }
}
