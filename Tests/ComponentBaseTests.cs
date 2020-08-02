using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Tests.Components
{
    class ComponentBaseTests : TestBase
    {
        MockComponent _base = new MockComponent();
        [SetUp]
        public void SetUp()
        {
            _base = (MockComponent)_game.Player.GetComponent<MockComponent>();
            _base.ProcessTimeUnit();
        }
        [Test] //todo: fix this test that fails sometimes
        public void NewComponentDerivedFromBaseTest()
        {
            Assert.AreEqual(1, _base.UpdateCounter);
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            _game.RunOnce();
            Assert.LessOrEqual(2, _base.UpdateCounter);
        }
    }
}
