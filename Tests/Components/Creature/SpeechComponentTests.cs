using Engine.Components.Creature;
using Engine.Entities;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Components.Creature
{
    class SpeechComponentTests : TestBase
    {
        SpeechComponent _base;
        string[] _answer;

        [Test]
        public void NewKeyBoardComponentTests()
        {
            _game = new MockGame(NewSpeechComponent);
            MockGame.RunOnce();
            MockGame.Stop();
        }
        private void NewSpeechComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _base = CreatureFactory.Person(new Coord()).GetGoRogueComponent<SpeechComponent>();
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
            _base = CreatureFactory.Person(new Coord()).GetGoRogueComponent<SpeechComponent>();
            _answer = _base.GetDetails();
            Assert.AreEqual(2, _answer.Length);
        }
    }
}
