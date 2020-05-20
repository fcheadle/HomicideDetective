using Engine.Entities;
using GoRogue;
using NUnit.Framework;
using SadConsole;

namespace Tests
{
    //[TestFixture]
    ////Can't test til a fix gets in that allows us to mock the game running better.... or i make one myself.
    class BasicEntityExtensionsTests
    {
        //[Test]
        public void GetCurrentRegionsTest()
        {
            BasicEntity entity = CreatureFactory.Person(new Coord(0, 0));
            BasicMap map = new BasicMap(7, 7, 7, Distance.EUCLIDEAN);
            Assert.Fail("working off Program.MapScreen.TownMap.Regions. Test was doomed from the start."); 
        }
    }
}
