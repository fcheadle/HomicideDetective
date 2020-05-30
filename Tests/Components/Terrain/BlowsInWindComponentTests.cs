using Engine.Components.Terrain;
using Engine.Entities;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Components.Terrain
{
    class BlowsInWindComponentTests : TestBase
    {
        TerrainFactory factory = new TerrainFactory();
        BlowsInWindComponent _base;
        string[] _answer;

        [Test]
        public void NewBlowsInWindComponentTests()
        {
            _game = new MockGame(NewKeyboardComponent);
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void NewKeyboardComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _base = factory.Grass(new Coord()).GetComponent<BlowsInWindComponent>();
            Assert.NotNull(_base);
        }

        [Test]
        public void GetDetailsTest()
        {
            _game = new MockGame(GetDetails);
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _base = factory.Grass(new Coord()).GetComponent<BlowsInWindComponent>();
            _answer = _base.GetDetails();
            Assert.AreEqual(1, _answer.Length);
        }
    }
}