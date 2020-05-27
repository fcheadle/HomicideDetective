using Engine.Components.Terrain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Components.Terrain
{
    class BlowsInWindComponentTests : TestBase
    {
        BlowsInWindComponent _base;
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
            _base = MockGame.Player.GetGoRogueComponent<BlowsInWindComponent>();
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
            _base = MockGame.Player.GetGoRogueComponent<BlowsInWindComponent>();
            _answer = _base.GetDetails();
            Assert.AreEqual(2, _answer.Length);
        }
    }
}
