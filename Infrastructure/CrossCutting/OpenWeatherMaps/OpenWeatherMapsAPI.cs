using Domain.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.CrossCutting.OpenWeatherMaps
{
    public class OpenWeatherMapsAPI
    {
        private string key = "ed50bf9fb5cdaf67eddd3e187161fd84";
        private string urlBase = "https://api.openweathermap.org/data/2.5/weather?";

        public async Task<Weather> GetWeather(string filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter))
                    throw new InvalidOperationException("City name is required");

                urlBase = urlBase + $@"q={filter}&APPID={key}";

                var url = new Uri(urlBase);

                var weather = new Weather();

                var client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Error: " + response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    var currentKelvin = json["main"]["temp"].ToString();
                    var humidity = json["main"]["humidity"].ToString();
                    var cloudiness = json["clouds"]["all"].ToString();

                    if (!string.IsNullOrEmpty(currentKelvin))
                        weather.CurrentTemperature = Convert.ToInt32(float.Parse(currentKelvin.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture) - 273.15);

                    if (!string.IsNullOrEmpty(humidity))
                        weather.CurrentHumidity = float.Parse(humidity.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);

                    if (!string.IsNullOrEmpty(cloudiness))
                        weather.Cloudiness = float.Parse(cloudiness.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                    //var itrm = JsonSerializer.Deserialize<string>(content);
                }

                return weather;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
