using Engine.Components.Creature;
using Engine.Entities;
using GoRogue;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using SadConsole;

namespace Tests.Entity
{
    [TestFixture]
    internal class CreatureFactoryTests : TestBase
    {
        static BasicEntity critter { get; set; }
        static string expectedName { get; } = "elder thing";

        [TearDown]
        public void TearDown()
        {
            MockGame.Stop();

        }

        private static void Person(GameTime time)
        {
            critter = CreatureFactory.Person(new Coord(0, 0));
        }
        [Test]
        public void PersonTest()
        {
            _game = new MockGame(Person);
            MockGame.RunOnce();
            Assert.IsNotNull((HealthComponent)critter.GetComponent<HealthComponent>(), "Person was born without a health component");
            Assert.IsNotNull((ActorComponent)critter.GetComponent<ActorComponent>(), "Person was born unable to move around or take actions");
        }
    }
}
