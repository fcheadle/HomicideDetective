using Microsoft.Xna.Framework;
using NUnit.Framework;
using Tests.Mocks;

//named such so that they are the first tests that execute
namespace Tests.Alpha
{
    class SanityChecks
    {
        private MockGame _game;

        [Test]//test that gameMock works as I expect it to
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
