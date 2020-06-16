using Engine.Creatures.Components;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Creature.Components
{
    class ThoughtComponentTests : TestBase
    {
        ThoughtsComponent _base;
        string[] _answer;

        [Test]
        public void NewThoughtsComponentTests()
        {
            _game = new MockGame(NewThoughtsComponent);
            _game.RunOnce();
            _game.Stop();
        }
        private void NewThoughtsComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _base = (ThoughtsComponent)_game.Player.GetComponent<ThoughtsComponent>();
            Assert.NotNull(_base);
        }

        [Test]
        public void ThinkThoughtTest()
        {
            _game = new MockGame(GetDetails);
            _game.RunOnce();
            Assert.AreEqual(0, _answer.Length);
            _base.Think("hello there.");
            _game.RunOnce();
            Assert.AreEqual(1, _answer.Length);
            _base.Think("oh, I'm alone...");
            _game.RunOnce();
            Assert.AreEqual(2, _answer.Length);
            _game.Stop();
        }
        [Test]
        public void ThinkThoughtsTest()
        {
            string[] thoughts =
            {
                "If Han Shot First",
                "Jet Fuel Can't Melt Steel Beams",
                "The CIA plotting against me",
                "I Hear it in my Fillings...",
            };
            _game = new MockGame(GetDetails);
            _game.RunOnce();
            Assert.AreEqual(0, _answer.Length);
            _base.Think(thoughts);
            _game.RunOnce();
            Assert.AreEqual(1, _answer.Length);
            _game.Stop();
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _base = (ThoughtsComponent)_game.Player.GetComponent<ThoughtsComponent>();
            _answer = _base.GetDetails();
        }

        [Test]//todo...
        public void ProcessTimeUnitTest()
        {
            _game = new MockGame(GetDetails);
            _game.RunOnce();
        }

        [Test]//todo...
        public void DecideWhatToDoTest()
        {
            Assert.Fail();
        }
    }
}
