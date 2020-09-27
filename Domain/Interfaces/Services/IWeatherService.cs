using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
    public interface IWeatherService : IBaseService<Weather>
    {
        Weather GetWeatherByFilter(string filter);
    }
}
