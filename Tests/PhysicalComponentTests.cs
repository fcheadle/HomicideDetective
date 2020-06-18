using Engine.Components;
using Engine.Items.Markings;
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
        [Test]
        public void MarkingsTest()
        {
            Assert.AreEqual(0, _component.MarkingsOn.Count);
            Assert.AreEqual(0, _component.MarkingsLeftBy.Count);

        }
        [Test]
        public void AddLimitedMarkingsTest()
        {
            Marking marking = new Marking();
            _component.AddLimitedMarkings(marking, 5);
            Assert.AreEqual(5, _component.MarkingsLeftBy.Count);
        }
        [Test]
        public void InteractTest()
        {
            Assert.Fail();
        }
        [Test]
        public void OnInteractTest()
        {
            Assert.Fail();
        }
        [Test]
        public void SetPhysicalDescriptionTest()
        {
            Assert.Fail();
        }
        [Test]
        public void ToStringOverrideTest()
        {
            Assert.Fail();
        }
    }
}
