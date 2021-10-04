using HomicideDetective.Places.Weather;
using Xunit;

namespace HomicideDetective.Tests.Places.Weather
{
    public class ReactsToWeatherComponentTests
    {
        [Fact]
        public void NewReactsToWeatherComponentTest()
        {
            var component = new ReactsToWeatherComponent(1, new[] {2, 3, 4, 5, 6});
            Assert.False(component.Animating);
        }
    }
}