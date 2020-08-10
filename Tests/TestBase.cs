using NUnit.Framework;
using Tests.Mocks;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Tests
{
    [TestFixture]
    [Category("RequiresGraphicsDevice")]
    public abstract class TestBase
    {
        public bool Finished { get; set; } = false;
        public bool Running { get; set; } = false;
        protected static MockGame Game;

        [OneTimeSetUp]
        public void Init()
        {
            Game = new MockGame(DoNothing);
            Game.RunOnce();
        }

        [OneTimeTearDown]
        public void End()
        {
            Game.Stop();
        }
        private void DoNothing(GameTime obj) { }
    }
}
