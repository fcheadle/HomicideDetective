using Engine.Scenes.Terrain;
using GoRogue;
using NUnit.Framework;
using SadConsole;

namespace Tests.Scenes.Components
{
    class TerrainFactoryTests : TestBase
    {
        TerrainFactory factory = new TerrainFactory();
        BasicTerrain _terrain;
        [SetUp]
        public void SetUp()
        {
            _terrain = factory.Generic(new Coord(0, 0), 14);
        }

        [Test]
        public void NewGenericTerrainTest()
        {
            Assert.NotNull(_terrain);
            Assert.AreEqual(new Coord(0, 0), _terrain.Position);
            Assert.AreEqual(14, _terrain.Glyph);
            Assert.IsTrue(_terrain.IsStatic);
            Assert.IsTrue(_terrain.IsTransparent);
            Assert.IsTrue(_terrain.IsWalkable);
        }
    }
}
