using System;
using HomicideDetective.Places.Weather;
using Xunit;

namespace HomicideDetective.Tests.Places.Weather
{
    public class WeatherControllerTests
    {
        [Fact]
        public void NewWeatherControllerTest()
        {
            var weather = new WeatherController();
            Assert.Equal(TimeSpan.Zero, weather.Elapsed);
        }
    }
}