using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.CrossCutting.OpenWeatherMaps;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Repositories
{
    public class WeatherRepository : BaseRepository<Weather>, IWeatherRepository
    {
        public Weather GetWeatherByFilter(string filter)
        {
            var openWeatherMaps = new OpenWeatherMapsAPI();

            var weather = openWeatherMaps.GetWeather(filter);

            return weather.Result;
        }
    }
}
