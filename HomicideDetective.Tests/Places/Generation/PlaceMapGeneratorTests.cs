using System.Collections.Generic;
using GoRogue.MapGeneration;
using HomicideDetective.Places.Generation;
using HomicideDetective.Places.Weather;
using SadRogue.Integration.Maps;
using Xunit;
// ReSharper disable UnusedMember.Local

namespace HomicideDetective.Tests.Places.Generation
{
    public class PlaceMapGeneratorTests
    {
        private int _width = 200;
        private int _height = 100;
        private int _viewWidth = 80;
        private int _viewHeight = 25;
        private TestHost _host = new TestHost();
        
        private void AssertMapHasRequiredComponents(RogueLikeMap map)
        {
            var weather = map.GoRogueComponents.GetFirstOrDefault<WeatherController>();
            Assert.NotNull(weather);
            var places = map.GoRogueComponents.GetFirstOrDefault<IEnumerable<Region>>();
            Assert.NotNull(places);    
        }
        
        [Fact]
        public void GenerateDownTownMapTest()
        {
            var map = PlaceMapGenerator.CreateDownTownMap(_width, _height, _viewWidth, _viewHeight);
            AssertMapHasRequiredComponents(map);
        }
        
        [Fact]
        public void GenerateHouseMapTest()
        {
            var map = PlaceMapGenerator.CreateDownTownMap(_width, _height, _viewWidth, _viewHeight);
            AssertMapHasRequiredComponents(map);
        }
        
        [Fact]
        public void GenerateParkMap()
        {
            var map = PlaceMapGenerator.CreateDownTownMap(_width, _height, _viewWidth, _viewHeight);
            AssertMapHasRequiredComponents(map);
        }
    }
}