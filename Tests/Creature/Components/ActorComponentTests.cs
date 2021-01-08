using HomicideDetective.Old.Creatures.Components;
using NUnit.Framework;

namespace Tests.Creature.Components
{
    class ActorComponentTests : TestBase
    {
        ActorComponent _base;
        string[] _answer;
        [SetUp]
        public void SetUp()
        {
            _base = (ActorComponent)Game.Player.GetComponent<ActorComponent>();
            _answer = _base.GetDetails();
        }
        [Test]
        public void NewActorComponentTests()
        {
            Assert.NotNull(_base);
            Assert.Less(0, _base.FovRadius);
        }
        [Test]
        public void GetDetailsTest()
        {
            Assert.AreEqual(2, _answer.Length);
        }
    }
}
