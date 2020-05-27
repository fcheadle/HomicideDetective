using Engine.Components.Creature;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Components.Creature
{
    class ThoughtComponentTests : TestBase
    {
        ThoughtsComponent _base;
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
            _base = MockGame.Player.GetGoRogueComponent<ThoughtsComponent>();
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
            _base = MockGame.Player.GetGoRogueComponent<ThoughtsComponent>();
            _answer = _base.GetDetails();
            Assert.AreEqual(2, _answer.Length);
        }
    }
}
