using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
    public class WeatherService : BaseService<Weather>, IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;

        public WeatherService(IWeatherRepository weatherRepository)
            : base()
        {
            _weatherRepository = weatherRepository;
        }

        public Weather GetWeatherByFilter(string filter)
        {
            return _weatherRepository.GetWeatherByFilter(filter);
        }
    }
}
