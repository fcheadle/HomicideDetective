using Engine.Maps;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Map
{
    class SceneMapTests : TestBase
    {
        SceneMap _map;
        [Test]
        public void NewSceneMapTest()
        {
            _game = new MockGame(NewSceneMap);
            _game.RunOnce();
            Assert.NotNull(_map);
        }

        private void NewSceneMap(GameTime obj)
        {
            _map = MockGame.Map;
            for (int i = 0; i < _map.Width; i++)
            {
                for (int j = 0; j < _map.Height; j++)
                {
                    Assert.NotNull(_map.GetTerrain<BasicTerrain>(i, j));
                }
            }
        }
    }
}
