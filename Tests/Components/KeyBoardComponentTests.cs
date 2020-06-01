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
            _game.RunOnce();
            _game.Stop();
        }
        private void NewKeyboardComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _base = (KeyboardComponent)MockGame.Player.GetComponent<KeyboardComponent>();
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
            _base = (KeyboardComponent)MockGame.Player.GetComponent<KeyboardComponent>();
            _answer = _base.GetDetails();
            Assert.AreEqual(2, _answer.Length);
        }
    }
}
