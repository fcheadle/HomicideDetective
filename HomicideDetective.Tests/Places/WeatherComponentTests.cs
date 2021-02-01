using HomicideDetective.Places;
using Xunit;

namespace HomicideDetective.Tests.Places
{
    public class WeatherComponentTests
    {
        [Fact(Skip = "nullref when we try to init place")]
        public void NewWeatherComponentTest()
        {
            var place = new Place(64,64, "testspace", "a space for tests");
            var component = new Weather(place);
            Assert.Equal("testspace", component.Name);
            Assert.Equal("The weather of testspace", component.Description);
        }
    }
}