using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Tests
{
    class MockTests
    {
        MockGame _game;

        private static void DummyUpdate(GameTime time)
        {
        }
        [Test]//test that gameMock works as I expect it to
        public void GameMockTest()
        {
            _game = new MockGame(DummyUpdate);
            Assert.DoesNotThrow(() => _game.RunOnce());
        }
    }
}
