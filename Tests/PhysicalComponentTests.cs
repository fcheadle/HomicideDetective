using Engine.Components;
using NUnit.Framework;

namespace Tests.Components
{
    class PhysicalComponentTests : TestBase
    {
        PhysicalComponent _component;
        [SetUp]
        public void SetUp()
        {
            _component = (PhysicalComponent)_game.Player.GetComponent<PhysicalComponent>();
            _component.ProcessTimeUnit();
        }
        [Test]
        public void NewPhysicalComponentTest()
        {
            Assert.NotNull(_component);
        }
        [Test]
        public void GetDetailsTest()
        {
            Assert.AreEqual(2, _component.GetDetails().Length);
        }
    }
}
