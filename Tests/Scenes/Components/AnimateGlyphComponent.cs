using Engine.Scenes.Components;
using Engine.Scenes.Terrain;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Scenes.Components
{
    class AnimateGlyphComponentTest : TestBase
    {
        TerrainFactory factory = new TerrainFactory();
        AnimateGlyphComponent _base;
        string[] _answer;

        [SetUp]
        public void SetUp()
        {
            _base = factory.Grass(new Coord()).GetComponent<AnimateGlyphComponent>();
        }
        [Test]
        public void NewAnimateGlyphComponent()
        {
            Assert.NotNull(_base);
        }

        [Test]
        public void GetDetailsTest()
        {
            _answer = _base.GetDetails();
            Assert.AreEqual(1, _answer.Length);
        }
    }
}