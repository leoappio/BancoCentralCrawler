using Microsoft.AspNetCore.Mvc;

namespace BancoCentralCrawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BancoCentralController : ControllerBase
    {
        public BancoCentralController()
        {
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

        }
    }
}
