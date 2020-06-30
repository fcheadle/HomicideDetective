using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;

namespace Tests.Alpha
{
    class SanityChecks
    {
        MockGame _game;

        [Test]//test that gameMock works as I expect it to
        public void GameMockTest()
        {
            _game = new MockGame(Update);
            Assert.DoesNotThrow(() => _game.RunOnce());
            Assert.Pass("Tests are valid and able to run!");
        }

        private void Update(GameTime obj)
        {
            
        }
    }
}
