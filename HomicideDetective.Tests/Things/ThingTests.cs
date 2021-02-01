using HomicideDetective.Things;
using SadRogue.Primitives;
using Xunit;

namespace HomicideDetective.Tests.Things
{
    public class ThingTests
    {
        private string _name = "highball";
        private string _description = "a hard spirit";
        private string _sizeDescr = "a double";
        private string _weightDescr = "is very stiff";
        private int _mass = 390;
        private int _volume = 265;
        
        [Fact]
        public void NewThingTest()
        {
            var thing = new Thing((1, 1), _name, _description, _mass, _volume, _sizeDescr, _weightDescr);
            Assert.Equal(new Point(1,1), thing.Position);
            Assert.Equal(_name, thing.Name);
            Assert.Equal(_description, thing.Description);
            Assert.Equal(390, thing.Substantive.Mass);
            Assert.Equal(265, thing.Substantive.Volume);
        }

        [Fact]
        public void GetDetailedDescriptionTest()
        {
            var thing = new Thing((1, 1), _name, _description, _mass, _volume, _sizeDescr, _weightDescr);
            var answer = thing.Substantive.GenerateDetailedDescription();
            Assert.Contains(_sizeDescr, answer);
            Assert.Contains(_weightDescr, answer);
        }

        [Fact]
        public void DetailsTest()
        {
            var thing = new Thing((1, 1), _name, _description, _mass, _volume, _sizeDescr, _weightDescr);
            var answer = thing.GetDetails();
            Assert.Contains(thing.Name, answer);
            Assert.Contains(thing.Description, answer);
            Assert.Contains($"Mass(g): {_mass}", answer);
            Assert.Contains($"Volume(ml): {_volume}", answer);
            Assert.Contains(thing.Substantive.GenerateDetailedDescription(), answer);
        }
    }
}