using Engine.Scenes.Terrain;
using GoRogue;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Scenes.Terrain
{
    class TerrainFactoryTests : TestBase
    {
        TerrainFactory factory = new TerrainFactory();
        BasicTerrain terrain;
        [TearDown]
        public void TearDown()
        {
            Assert.IsTrue(terrain.IsStatic);
            Assert.IsTrue(terrain.IsTransparent);
            Assert.IsTrue(terrain.IsWalkable);
        }

        [Test]
        public void NewGenericTerrainTest()
        {
            _game = new MockGame(NewGenericTerrain);
            _game.RunOnce();
            _game.Stop();
        }
        public void NewGenericTerrain(GameTime time)
        {
            terrain = factory.Generic(new Coord(0, 0), 14);
            Assert.NotNull(terrain);
            Assert.AreEqual(new Coord(0, 0), terrain.Position);
            Assert.AreEqual(14, terrain.Glyph);
        }
    }
}
