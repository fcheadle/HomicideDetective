using Engine.Components;
using Engine.Components.Creature;
using Engine.Components.UI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Components.UI
{
    class DisplayComponentTests : TestBase
    {
        DisplayComponent<HealthComponent> _base;
        string[] _answer;

        [Test]
        public void NewKeyBoardComponentTests()
        {
            _game = new MockGame(NewKeyboardComponent);
            _game.RunOnce();
            _game.Stop();
        }
        private void NewKeyboardComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _base = (DisplayComponent<HealthComponent>)MockGame.Player.GetComponent<DisplayComponent<HealthComponent>>();
            Assert.NotNull(_base);
            _base.ProcessTimeUnit();
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
            _base = (DisplayComponent<HealthComponent>)MockGame.Player.GetComponent<DisplayComponent<HealthComponent>>();
            _answer = _base.GetDetails();
            HealthComponent c = (HealthComponent)MockGame.Player.GetComponent<HealthComponent>();
            Assert.AreEqual(c.GetDetails().Length, _answer.Length);
        }
    }
}
