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

        [SetUp]
        public void SetUp()
        {
            _base = (ThoughtsComponent)_game.Player.GetComponent<ThoughtsComponent>();
            _answer = _base.GetDetails();
        }

        [Test]
        public void NewThoughtsComponentTests()
        {
            Assert.NotNull(_base);
        }
        [Test]
        public void ThinkThoughtsTest()
        {
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            Assert.AreEqual(0, _answer.Length);
            _base.Think("hello there.");
            _game.RunOnce();
            _answer = _base.GetDetails();
            Assert.AreEqual(1, _answer.Length);
            _base.Think("oh, I'm alone...");
            _game.RunOnce();
            _answer = _base.GetDetails();
            Assert.AreEqual(2, _answer.Length);
            string[] thoughts =
            {
                "If Han Shot First",
                "Jet Fuel Can't Melt Steel Beams",
                "The CIA plotting against me",
                "I Hear it in my Fillings...",
            };
            _base.Think(thoughts);
            _game.RunOnce();
            _answer = _base.GetDetails();
            Assert.AreEqual(4, _answer.Length);

            _base.Think(thoughts);
            _answer = _base.GetDetails();
            Assert.AreEqual(4, _answer.Length);

        }

        [Test]//todo...
        public void ProcessTimeUnitTest()
        {
            Assert.DoesNotThrow(() => _base.ProcessTimeUnit());
        }

        [Test]//todo...
        public void DecideWhatToDoTest()
        {
            Assert.Fail();
        }
    }
}
