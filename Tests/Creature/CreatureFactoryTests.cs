using HomicideDetective.Old.Creatures;
using HomicideDetective.Old.Creatures.Components;
using GoRogue;
using NUnit.Framework;
using SadConsole;

namespace Tests.Creature
{
    [TestFixture]
    internal class CreatureFactoryTests : TestBase
    {
        static BasicEntity Critter { get; set; }
        DefaultCreatureFactory _factory = new DefaultCreatureFactory();

        [Test]
        public void PersonTest()
        {
            Critter = _factory.Person(new Coord(0, 0));
            Assert.IsNotNull((HealthComponent)Critter.GetComponent<HealthComponent>(), "Person was born without a health component");
            Assert.IsNotNull((ActorComponent)Critter.GetComponent<ActorComponent>(), "Person was born unable to move around or take actions");
        }
    }
}
