using Engine.Components;
using Engine.Items.Markings;
using NUnit.Framework;
using SadConsole;

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
            Marking marking = new Marking()
            {
                Name = "bruise",
                Color = "light purple",
                Description = "just past the {bodyPart}",
                Adjective = "round"
            };
            _component.AddLimitedMarkings(marking, 5);
            Assert.AreEqual(5, _component.MarkingsLeftBy.Count);
        }
        [Test]
        public void AddUnlimitedMarkingsTest()
        {
            Marking marking = new Marking()
            {
                Name = "hair follicle",
                Color = "red",
                Description = "short",
                Adjective = "curly"
            };
            int markingsBefore = _component.MarkingsLeftBy.Count;
            _component.AddUnlimitedMarkings(marking);
            Assert.AreEqual(markingsBefore + 1, _component.MarkingsLeftBy.Count);
        }
        [Test]
        public void InteractTest()
        {
            Marking hair = new Marking() 
            {
                Name = "hair follicle",
                Description = "short",
                Adjective = "dirty"
            };

            BasicEntity raccoon = MockGame.CreatureFactory.Animal(new GoRogue.Coord());
            raccoon.Name = "trash panda"; //for fun :)
            _component = (PhysicalComponent)raccoon.GetComponent<PhysicalComponent>();

            Marking dirt = new Marking()
            {
                Name = "dirt",
                Description = "course yet loamy",
                Adjective = "moist",
                Color = "grey & brown"
            };

            _component.AddLimitedMarkings(dirt, 100);

            Marking grease = new Marking()
            {
                Name = "grease",
                Description = "a smudging",
                Adjective = "sticky",
                Color = "yellow"
            };

            _component.AddLimitedMarkings(grease, 100);


            BasicEntity shinyThing = MockGame.ItemFactory.Generic(new GoRogue.Coord(), "shiny thing");
            PhysicalComponent shinyThingComponent = (PhysicalComponent)shinyThing.GetComponent<PhysicalComponent>();

            //interact
            shinyThingComponent.Interact(_component.Interact());
            //assert on markings
            Assert.AreEqual(1, shinyThingComponent.MarkingsOn.Count);

            for (int i = 0; i < 100; i++)
            {
                shinyThingComponent.Interact(_component.Interact());
            }

            Assert.AreEqual(101, shinyThingComponent.MarkingsOn.Count);
            Assert.AreEqual(99, _component.MarkingsLeftBy.Count);
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
