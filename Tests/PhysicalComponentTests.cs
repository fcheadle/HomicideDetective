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
        public void MarkingsOnTest()
        {
            Assert.AreEqual(0, _component.MarkingsOn.Count);
            _component.Mark(new Marking());
            Assert.AreEqual(1, _component.MarkingsOn.Count);
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
            //something that produces markings

            //something that can accept markings

            //interact

            //assert on markings
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
            _component.SetPhysicalDescription("A short, sturdy creature fond of drink and industry");
            Assert.AreEqual("A short, sturdy creature fond of drink and industry", _component.Description);
        }
        [Test]
        public void ToStringOverrideTest()
        {
            string answer = _component.ToString();
            Assert.False(answer.Contains("PhysicalComponent"));
            Assert.False(answer.ToLower().Contains("physicalcomponent"));
        }
    }
}
