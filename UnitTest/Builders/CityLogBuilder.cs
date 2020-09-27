using Bogus;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest.Builders
{
    public class CityLogBuilder
    {
        private readonly Faker<CityLog> _faker = new Faker<CityLog>("pt_BR");

        private IEnumerable<CityLog> Builder(int count)
        {
            var list = this._faker.RuleFor(p => p.NameWithoutAccent, p => p.Address.State().ToLower())
                                  .RuleFor(p => p.Count, p => p.Random.Number(50))
                                  .RuleFor(p => p.Date, p => p.Date.Past())
                                  .GenerateLazy(count);

            return list;
        }

        public IEnumerable<CityLog> GetCityLogBuilderList()
        {
            return this.Builder(5);
        }

        public CityLog GetCityLogBuilder()
        {
            return this.Builder(1).Last();
        }

        public IEnumerable<CityLog> GetCityLogBuilderEmptyList()
        {
            return new List<CityLog>();
        }
    }
}
