using HomicideDetective.Old.Tests.Mocks;
using NUnit.Framework;

namespace HomicideDetective.Old.Tests.Alpha
{
    class ComponentBaseTests : TestBase
    {
        MockComponent _base = new MockComponent();
        [SetUp]
        public void SetUp()
        {
            _base = (MockComponent)Game.Player.GetComponent<MockComponent>();
            _base.ProcessTimeUnit();
        }
        [Test] //todo: fix this test that fails sometimes
        public void NewComponentDerivedFromBaseTest()
        {
            Assert.AreEqual(1, _base.UpdateCounter);
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Game.RunOnce();
            Assert.LessOrEqual(2, _base.UpdateCounter);
        }
    }
}
