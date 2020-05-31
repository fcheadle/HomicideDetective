using Engine.Components;
using NUnit.Framework;

namespace Tests.Components
{
    class PhysicalComponentTests : TestBase
    {
        PhysicalComponent _component;

        [Test]
        public void NewPhysicalComponentTest()
        {
            _game = new MockGame(NewPhysicalComponent);
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void NewPhysicalComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (PhysicalComponent)MockGame.Player.GetComponent<PhysicalComponent>();
            _component.ProcessTimeUnit();
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
            _component = (PhysicalComponent)MockGame.Player.GetComponent<PhysicalComponent>();
            _component.ProcessTimeUnit();
        }
    }
}
