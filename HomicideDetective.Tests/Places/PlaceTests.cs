using GoRogue.MapGeneration;
using HomicideDetective.Places;
using Xunit;

namespace HomicideDetective.Tests.Places
{
    public class PlaceTests
    {
        private readonly ISubstantive.Types _type = ISubstantive.Types.Place;
        private readonly string _name = "montana";
        private readonly string _description = "montana";

        private Region TestRegion() 
            => new Region(_name, (0, 0), (15, 0), (15, 15), (0, 15));
        
        [Fact]
        public void NewPlaceTest()
        {
            var substantive = new Substantive(_type, _name, description: _description);
            var place = new Place(TestRegion(), substantive);
            Assert.NotNull(place.Markings);
            Assert.Empty(place.Markings.MarkingsOn);
            Assert.Empty(place.Markings.MarkingsLeftBy);
        }
    }
}