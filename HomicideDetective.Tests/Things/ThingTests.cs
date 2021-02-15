using HomicideDetective.Mysteries;
using HomicideDetective.People;
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
            var subs = new Substantive(Substantive.Types.Thing, _name, 16, null, "a", "it", 
                "its", _description, _mass, _volume, _sizeDescr, _weightDescr);
            
            var thing = new Thing((1,1), subs);
            
            Assert.Equal(new Point(1,1), thing.Position);
            Assert.Equal(_name, thing.Name);
            Assert.Equal(_description, thing.Description);
            Assert.Equal(390, thing.Substantive.Mass);
            Assert.Equal(265, thing.Substantive.Volume);
        }
    }
}