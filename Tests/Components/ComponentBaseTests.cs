using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Tests.Components
{
    class ComponentBaseTests : TestBase
    {
        MockComponent _base = new MockComponent();

        [Test]
        public void NewComponentDerivedFromBaseTest()
        {
            _game = new MockGame(NewComponentDerivedFromBased);
            MockGame.RunOnce();
            Assert.AreEqual(1, _base.UpdateCounter);
            MockGame.RunOnce();
            Assert.Less(2, _base.UpdateCounter);
            MockGame.Stop();
        }
        private void NewComponentDerivedFromBased(GameTime time)
        {
            _base = (MockComponent)MockGame.Player.GetComponent<MockComponent>();
            _base.ProcessTimeUnit();
        }
    }
}
