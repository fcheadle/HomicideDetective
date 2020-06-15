using Engine.Creatures.Components;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Components.Creature
{
    class ActorComponentTests : TestBase
    {
        ActorComponent _base;
        string[] _answer;

        [Test]
        public void NewActorComponentTests()
        {
            _game = new MockGame(NewActorComponent);
            _game.RunOnce();
            _game.Stop();
        }
        private void NewActorComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _base = (ActorComponent)_game.Player.GetComponent<ActorComponent>();
            Assert.NotNull(_base);
        }
        [Test]
        public void GetDetailsTest()
        {
            _game = new MockGame(GetDetails);
            _game.RunOnce();
            _game.Stop();
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _base = (ActorComponent)_game.Player.GetComponent<ActorComponent>();
            _answer = _base.GetDetails();
            Assert.AreEqual(2, _answer.Length);
        }
    }
}
