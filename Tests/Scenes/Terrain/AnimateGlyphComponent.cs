using Engine.Scenes.Terrain;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Scenes.Terrain
{
    class AnimateGlyphComponent : TestBase
    {
        TerrainFactory factory = new TerrainFactory();
        Engine.Scenes.Terrain.Components.AnimateGlyphComponent _base;
        string[] _answer;

        [Test]
        public void NewBlowsInWindComponentTests()
        {
            _game = new MockGame(NewBlowsInWindComponent);
            _game.RunOnce();
            _game.Stop();
        }
        private void NewBlowsInWindComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _base = factory.Grass(new Coord()).GetComponent<Engine.Scenes.Terrain.Components.AnimateGlyphComponent>();
            Assert.NotNull(_base);
        }

        [Test]
        public void GetDetailsTest()
        {
            _game = new MockGame(GetDetails);
            _game.RunOnce();
            _game.Stop();
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _base = factory.Grass(new Coord()).GetComponent<Engine.Scenes.Terrain.Components.AnimateGlyphComponent>();
            _answer = _base.GetDetails();
            Assert.AreEqual(1, _answer.Length);
        }
    }
}