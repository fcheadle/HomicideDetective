using Engine.Components;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Components
{
    class KeyBoardComponentTests : TestBase
    {
        KeyboardComponent _base;
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
            _base = MockGame.Player.GetGoRogueComponent<KeyboardComponent>();
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
            _base = MockGame.Player.GetGoRogueComponent<KeyboardComponent>();
            _answer = _base.GetDetails();
            Assert.AreEqual(2, _answer.Length);
        }
    }
}
