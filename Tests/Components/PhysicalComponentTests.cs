using Engine.Components;
using NUnit.Framework;

namespace Tests.Components
{
    class PhysicalComponentTests : TestBase
    {
        PhysicalComponent _base = new PhysicalComponent();

        [Test]
        public void NewPhysicalComponentTest()
        {
            _game = new MockGame(NewPhysicalComponent);
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void NewPhysicalComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _base = MockGame.Player.GetGoRogueComponent<PhysicalComponent>();
            _base.ProcessGameFrame();
        }
        [Test]
        public void GetDetailsTest()
        {
            _game = new MockGame(GetDetails);
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _base = MockGame.Player.GetGoRogueComponent<PhysicalComponent>();
            _base.ProcessGameFrame();
        }
    }
}
