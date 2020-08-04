using Engine.Scenes;
using Engine.Scenes.Areas;
using GoRogue;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using SadConsole;
using System;
using Tests.Mocks;

//named such so that they are the first tests that execute
namespace Tests.Alpha
{
    class SanityChecks
    {
        MockGame _game;
        double degrees = 0.0;
        SceneMap originalMap;
        House backrooms;

        [Test]//test that gameMock works as I expect it to
        [Category("NonGraphical")]
        public void GameMockTest()
        {
            _game = new MockGame(Update);
            Assert.DoesNotThrow(() => _game.RunOnce());
            Assert.Pass("Tests are valid and able to run!");
            _game.Stop();
        }

        private void Update(GameTime obj)
        {
            
        }

        //[Test] //for manual use
        //public void RunTestGame()
        //{
        //    _game = new MockGame(RotateTest);
        //    SadConsole.Game.OnInitialize = Backrooms;
        //    _game.RunOnce();

        //    originalMap = MockGame.UIManager.Map;
        //    _game.Start();
            
        //}

    }
}
