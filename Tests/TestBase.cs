using NUnit.Framework;
using System;
using System.Collections.Generic;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Tests
{
    [TestFixture]
    [Category("RequiresGraphicsDevice")]
    abstract class TestBase
    {
        public bool Finished { get; set; } = false;
        public bool Running { get; set; } = false;

        protected List<Action<GameTime>> _completedTests = new List<Action<GameTime>>();
        protected List<Action<GameTime>> _notCompletedTests = new List<Action<GameTime>>();
        protected static MockGame _game;

        [OneTimeSetUp]
        public void Init()
        {
            _game = new MockGame(DoNothing);
            _game.RunOnce();
        }

        [OneTimeTearDown]
        public void End()
        {
            _game.Stop();
        }
        private void DoNothing(GameTime obj) { }
    }
}
