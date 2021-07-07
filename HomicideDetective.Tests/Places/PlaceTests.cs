using GoRogue.MapGeneration;
using HomicideDetective.Places;
using Xunit;

namespace HomicideDetective.Tests.Places
{
    public class PlaceTests
    {
        private readonly string _name = "montana";
        private readonly string _description = "montana";
        private readonly string _noun = "state";

        private Region TestRegion() 
            => new Region(_name, (0, 0), (15, 0), (15, 15), (0, 15));
        
        [Fact]
        public void NewPlaceTest()
        {
            var place = new Place(TestRegion(), _name, _description, _noun);
            Assert.NotNull(place.Markings);
            Assert.Empty(place.Markings.MarkingsOn);
            Assert.Empty(place.Markings.MarkingsLeftBy);
        }
    }
}