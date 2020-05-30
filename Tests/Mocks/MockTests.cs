using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Tests
{
    class MockTests
    {
        MockGame _game;

        private static void DummyUpdate(GameTime time)
        {
            Assert.Pass("We are calling our update method successfully");
            MockGame.Stop();
        }
        [Test]//test that gameMock works as I expect it to
        public void GameMockTest()
        {
            _game = new MockGame(DummyUpdate);
            MockGame.Start();
        }
    }
}
