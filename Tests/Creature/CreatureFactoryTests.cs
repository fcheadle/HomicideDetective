using Engine.Creatures;
using Engine.Creatures.Components;
using GoRogue;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using SadConsole;

namespace Tests.Creature
{
    [TestFixture]
    internal class CreatureFactoryTests : TestBase
    {
        static BasicEntity critter { get; set; }
        static string expectedName { get; } = "elder thing";
        CreatureFactory _factory = new CreatureFactory();

        [Test]
        public void PersonTest()
        {
            critter = _factory.Person(new Coord(0, 0));
            Assert.IsNotNull((HealthComponent)critter.GetComponent<HealthComponent>(), "Person was born without a health component");
            Assert.IsNotNull((ActorComponent)critter.GetComponent<ActorComponent>(), "Person was born unable to move around or take actions");
        }
    }
}
