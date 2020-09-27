using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IWeatherRepository
    {
        public Weather GetWeatherByFilter(string filter);
    }
}
