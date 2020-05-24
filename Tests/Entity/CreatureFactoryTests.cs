using Engine.Components.Creature;
using Engine.Entities;
using GoRogue;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Entity
{
    [TestFixture]
    internal class CreatureFactoryTests : TestBase
    {
        static BasicEntity critter { get; set; }
        static string expectedName { get; } = "elder thing";
        private static void Update(GameTime time)
        {
            critter = CreatureFactory.Person(new Coord(0, 0));
        }
        [Test]
        public void PersonTest()
        {
            _game = new MockGame(Update);
            MockGame.RunOnce();
            Assert.IsNotNull(critter.GetGoRogueComponent<HealthComponent>(), "Person was born without a health component");
            Assert.IsNotNull(critter.GetGoRogueComponent<ActorComponent>(), "Person was born unable to move around or take actions");
            MockGame.Stop();
        }
    }
}
