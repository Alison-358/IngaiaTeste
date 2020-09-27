using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Weather
    {
        public int CurrentTemperature { get; set; }
        public float CurrentHumidity { get; set; }
        public float Cloudiness { get; set; }
    }
}
