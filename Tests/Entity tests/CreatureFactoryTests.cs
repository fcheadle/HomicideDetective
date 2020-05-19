using Engine.Components.Creature;
using Engine.Entities;
using GoRogue;
using NUnit.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Entity_tests
{
    [TestFixture]
    public class CreatureFactoryTests
    {
        //[Test] //doesnt work due to some sadconsole business
        public void PersonTest()
        {
            BasicEntity person = CreatureFactory.Person(new Coord(0, 0));
            Assert.IsNotNull(person.GetGoRogueComponent<HealthComponent>());
            Assert.IsNotNull(person.GetGoRogueComponent<ActorComponent>());
            Assert.IsNotNull(person.GetGoRogueComponent<SpeechComponent>());
        }
    }
}
