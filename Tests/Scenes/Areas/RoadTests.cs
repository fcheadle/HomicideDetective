using HomicideDetective.Old.Scenes.Areas;
using GoRogue;
using NUnit.Framework;

namespace Tests.Scenes.Areas
{
    [TestFixture]
    public class RoadTests
    {
        Road _road;
        Coord start = new Coord(15, 0);
        Coord stop = new Coord(35, 20);
        [Test]
        public void NewHorizontlRoadTest()
        {
            _road = new Road(start, stop, RoadNumbers.Thirteenth);
            Assert.AreEqual(_road.Name, RoadNumbers.Thirteenth.ToString());
            Assert.Less(_road.OuterPoints.Count, _road.InnerPoints.Count);
        }
        [Test]
        public void NewVerticalRoadTest()
        {
            _road = new Road(start, stop, RoadNames.ZooFront);
            Assert.AreEqual(_road.Name, RoadNames.ZooFront.ToString());
            Assert.Less(_road.OuterPoints.Count, _road.InnerPoints.Count);
        }
    }
}
