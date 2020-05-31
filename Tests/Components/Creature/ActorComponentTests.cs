using Engine.Components.Creature;
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
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void NewActorComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _base = (ActorComponent)MockGame.Player.GetComponent<ActorComponent>();
            Assert.NotNull(_base);
        }
        [Test]
        public void GetDetailsTest()
        {
            _game = new MockGame(GetDetails);
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void GetDetails(Microsoft.Xna.Framework.GameTime time)
        {
            _base = (ActorComponent)MockGame.Player.GetComponent<ActorComponent>();
            _answer = _base.GetDetails();
            Assert.AreEqual(2, _answer.Length);
        }
    }
}
