using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Places;
using SadRogue.Primitives;
using Xunit;

namespace HomicideDetective.Tests.Places
{
    public class PlaceCollectionTests
    {
        public static readonly IEnumerable<object[]> NonContainingPoints = new List<object[]>
        {
            new object[] {new Point(0, 5)}, 
            new object[] {new Point(-1, 1)}, 
            new object[] {new Point(1, 1)}, 
            new object[] {new Point(60, 60)}, 
        };
        public static readonly IEnumerable<object[]> ContainingPoints = new List<object[]>
        {
            new object[] {new Point(0,0)}, 
            new object[] {new Point(3, 1)}, 
            new object[] {new Point(36, 29)}, 
        };
        private Place CreatePlace(Point sw)
        {
            var region = CreateRegion(sw);
            var subs = CreateSubstantive();
            return new Place(region, subs);
        }
        private Region CreateRegion(Point sw)
            => new Region($"room {sw}", sw + (0, -15), sw + (15, -15), sw + (15, 0), sw);

        private Substantive CreateSubstantive() => new Substantive(ISubstantive.Types.Place, "test-zone");
        
        private PlaceCollection CreateCollection()
        {
            var collection = new PlaceCollection();
            for(int i = 0; i < 12; i++)
                collection.Add(CreatePlace((i * 3, i * 3)));

            return collection;
        }
        
        [Fact]
        public void ContainsPointTest()
        {
            var collection = CreateCollection();
            
            foreach(var point in NonContainingPoints)
                Assert.False(collection.Contains((Point)point[0]));
            foreach(var point in ContainingPoints)
                Assert.True(collection.Contains((Point)point[0]));
        }
        
        [Fact]
        public void GetContainingRegionsTest()
        {
            var collection = CreateCollection();
            Assert.Single(collection.GetPlacesContaining((0, 0)));
            Assert.True(collection.GetPlacesContaining((3,0)).Count() == 2);
            Assert.True(collection.GetPlacesContaining((6,0)).Count() == 3);
            
            Assert.Empty(collection.GetPlacesContaining((-1, -1)));
            Assert.Empty(collection.GetPlacesContaining((60, 60)));
            Assert.Empty(collection.GetPlacesContaining((3, 5)));
        }
    }
}