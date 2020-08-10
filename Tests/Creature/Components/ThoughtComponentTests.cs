using Engine.Creatures.Components;
using NUnit.Framework;

namespace Tests.Creature.Components
{
    class ThoughtComponentTests : TestBase
    {
        ThoughtsComponent _base;
        string[] _answer;

        [SetUp]
        public void SetUp()
        {
            _base = (ThoughtsComponent)Game.Player.GetComponent<ThoughtsComponent>();
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
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Assert.AreEqual(0, _answer.Length);
            _base.Think("hello there.");
            Game.RunOnce();
            _answer = _base.GetDetails();
            Assert.AreEqual(1, _answer.Length);
            _base.Think("oh, I'm alone...");
            Game.RunOnce();
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
            Game.RunOnce();
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
            Assert.DoesNotThrow(() => _base.DecideWhatToDo());
        }
    }
}
