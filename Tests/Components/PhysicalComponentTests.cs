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
            _game.RunOnce();
            _game.Stop();
        }
        private void NewPhysicalComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (PhysicalComponent)_game.Player.GetComponent<PhysicalComponent>();
            _component.ProcessTimeUnit();
        }
        [Test]
        public void GetDetailsTest()
        {
            _game = new MockGame(GetDetails);
            _game.RunOnce();
            _game.Stop();
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (PhysicalComponent)_game.Player.GetComponent<PhysicalComponent>();
            _component.ProcessTimeUnit();
        }
    }
}
