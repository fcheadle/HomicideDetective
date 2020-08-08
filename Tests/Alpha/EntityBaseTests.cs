using Engine;
using Engine.Items.Markings;
using GoRogue;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Tests.Alpha
{
    class EntityBaseTests : TestBase
    {
        private EntityBase _base;
        [SetUp]
        public void SetUp()
        {
            _base = new EntityBase(Color.Aqua, Color.Black, 1, Coord.NONE,1,true, true);
            
        }
        [Test]
        public void NewPhysicalComponentTest()
        {
            Assert.NotNull(_base);
        }
        [Test]
        public void MarkingsOnTest()
        {
            Assert.AreEqual(0, _base.MarkingsOn.Count);
            _base.Mark(new Marking());
            Assert.AreEqual(1, _base.MarkingsOn.Count);
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
            _base.AddLimitedMarkings(marking, 5);
            Assert.AreEqual(5, _base.MarkingsLeftBy.Count);
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
            int markingsBefore = _base.MarkingsLeftBy.Count;
            _base.AddUnlimitedMarkings(marking);
            Assert.AreEqual(markingsBefore + 1, _base.MarkingsLeftBy.Count);
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

            EntityBase raccoon = MockGame.CreatureFactory.Animal(new GoRogue.Coord());
            raccoon.Name = "trash panda"; //for fun :)

            Marking dirt = new Marking()
            {
                Name = "dirt",
                Description = "course yet loamy",
                Adjective = "moist",
                Color = "grey & brown"
            };

            raccoon.AddLimitedMarkings(dirt, 100);

            Marking grease = new Marking()
            {
                Name = "grease",
                Description = "a smudging",
                Adjective = "sticky",
                Color = "yellow"
            };

            raccoon.AddLimitedMarkings(grease, 100);


            EntityBase shinyThing = MockGame.ItemFactory.Generic(new GoRogue.Coord(), "shiny thing");
            //interact
            raccoon.InteractWith(shinyThing);
            //assert on markings
            Assert.AreEqual(1, shinyThing.MarkingsOn.Count);

            for (int i = 0; i < 100; i++)
            {
                raccoon.InteractWith(shinyThing);
            }

            Assert.AreEqual(101, shinyThing.MarkingsOn.Count);
            Assert.AreEqual(99, raccoon.MarkingsLeftBy.Count);
        }
        [Test]
        public void ToStringOverrideTest()
        {
            string answer = _base.ToString();
            Assert.False(answer.Contains("PhysicalComponent"));
            Assert.False(answer.ToLower().Contains("physicalcomponent"));
        }
    }
}
