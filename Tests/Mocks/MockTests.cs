using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;

namespace Tests
{
    class MockTests
    {
        MockGame _game;

        [Test]//test that gameMock works as I expect it to
        public void GameMockTest()
        {
            _game = new MockGame(Update);
            Assert.DoesNotThrow(() => _game.RunOnce());
        }

        private void Update(GameTime obj)
        {
            
        }
    }
}
