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

        private Region TestRegion() 
            => new Region(_name, (0, 0), (15, 0), (15, 15), (0, 15));
        
        [Fact]
        public void NewPlaceTest()
        {
            var place = new Place(TestRegion(), _name, _description, _properties, _noun, _pronoun);
            Assert.NotNull(place.Markings);
            Assert.Empty(place.Markings.MarkingsOn);
            Assert.Empty(place.Markings.MarkingsLeftBy);
        }
    }
}