using System;
using UnitTest.Builders;
using Xunit;

namespace UnitTest
{
    public class CityLogBuilder_Test
    {
        private readonly CityLogBuilder _cityLogBuilder = new CityLogBuilder();

        [Fact]
        public void Should_Invoke_A_Error_List()
        {
            var cityLogs = this._cityLogBuilder.GetCityLogBuilderList();

            Assert.Null(cityLogs);
        }

        [Fact]
        public void Should_Invoke_A_EmptyList()
        {
            var cityLogs = this._cityLogBuilder.GetCityLogBuilderEmptyList();

            Assert.Empty(cityLogs);
        }

        [Fact]
        public void Should_Invoke_A_List()
        {
            var cityLogs = this._cityLogBuilder.GetCityLogBuilderList();

            Assert.NotEmpty(cityLogs);
        }
    }
}
