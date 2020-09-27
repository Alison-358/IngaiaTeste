using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class CityLog
    {
        public int Count { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public string NameWithoutAccent { get; set; }
    }
}
