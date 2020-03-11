using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private string _currentTemperatureUrl = "http://api.weatherunlocked.com/api/current/{0}?app_id={1}&app_key={2}";
        private string _currentTemperatureRequest;

        public WeatherController(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _config = config;

            var location = "45.302940,-122.777992"; // Wilsonville
            var appId = _config["WeatherService:AppId"];
            var appKey = _config["WeatherService:AppKey"];

            _clientFactory = clientFactory;
            
            _currentTemperatureRequest = string.Format(_currentTemperatureUrl, location, appId, appKey);
        }

        [HttpGet]
        [Route("temperature/current")]
        public ActionResult<CurrentTemperature> GetCurrentTemperature()
        {
            try
            {
                var currentTemperature =  getCurrentTemperature();

                return Ok(currentTemperature);
            }
            catch(ApplicationException aex)
            {
                // Log failure later

                return StatusCode(500);
            }
        }

        internal CurrentTemperature getCurrentTemperature()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _currentTemperatureRequest);

            var client = _clientFactory.CreateClient();

            var response = client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseStream = response.Content.ReadAsStreamAsync().Result;
                var currentWeather = JsonSerializer.DeserializeAsync<CurrentWeather>(responseStream).Result;
                
                return new CurrentTemperature
                    {
                        C = currentWeather.temp_c,
                        F = currentWeather.temp_f
                    };
            }
            else
            {
                throw new ApplicationException("Failed to retrieve current temperature");
            }                               
        }

        internal class CurrentWeather {
            public decimal temp_c {get; set;}
            public decimal temp_f {get; set;}
        }

        public class CurrentTemperature {
            public decimal F {get; set;}
            public decimal C {get; set;}
        }
    }
}
