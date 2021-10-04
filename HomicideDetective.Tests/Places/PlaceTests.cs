using GoRogue.MapGeneration;
using HomicideDetective.Places;
using HomicideDetective.Words;
using Xunit;

namespace HomicideDetective.Tests.Places
{
    public class PlaceTests
    {
        private readonly string _name = "montana";
        private readonly string _description = "montana";
        private readonly Noun _noun = new ("state", "states");
        private readonly Pronoun _pronoun = Constants.ItemPronouns;
        private readonly PhysicalProperties _properties = new PhysicalProperties(18, 24, "superior", "big", "young", "round", "blue", "Royal");
        private PolygonArea TestPolygon => new PolygonArea((0, 0), (15, 0), (15, 15), (0, 15));
        private Region TestRegion() => new Region(TestPolygon);
        
        [Fact]
        public void NewPlaceTest()
        {
            var place = new Place(TestRegion().Area, _name, _description, _noun, _pronoun, _properties);
            Assert.NotNull(place.Markings);
            Assert.Empty(place.Markings.MarkingsOn);
            Assert.Empty(place.Markings.MarkingsLeftBy);
        }
    }
}