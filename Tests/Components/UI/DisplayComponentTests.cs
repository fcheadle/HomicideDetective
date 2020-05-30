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
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void NewKeyboardComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _base = MockGame.Player.GetGoRogueComponent<DisplayComponent<HealthComponent>>();
            Assert.NotNull(_base);
            _base.ProcessGameFrame();
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
            _base = MockGame.Player.GetGoRogueComponent<DisplayComponent<HealthComponent>>();
            _answer = _base.GetDetails();
            Assert.AreEqual(3, _answer.Length);
        }
    }
}
