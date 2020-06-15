using Engine.Components.Creature;
using Engine.Entities.Creatures;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Components.Creature
{
    class SpeechComponentTests : TestBase
    {
        SpeechComponent _component;
        string[] _answer;
        CreatureFactory _factory = new CreatureFactory();

        [Test]
        public void NewSpeechComponentTest()
        {
            _game = new MockGame(NewSpeechComponent);
            _game.RunOnce();
            _game.Stop();
        }
        private void NewSpeechComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (SpeechComponent)_factory.Person(new Coord()).GetComponent<SpeechComponent>();
            Assert.NotNull(_component);
            Assert.NotNull(_component.TendencyToMinimize);
            Assert.Less(0, _component.TendencyToMinimize);
            Assert.NotNull(_component.TendencyToInvalidate);
            Assert.Less(0, _component.TendencyToInvalidate);
            Assert.NotNull(_component.TendencyToDeny);
            Assert.Less(0, _component.TendencyToDeny);
            Assert.NotNull(_component.TendencyToJustify);
            Assert.Less(0, _component.TendencyToJustify);
            Assert.NotNull(_component.TendencyToArgue);
            Assert.Less(0, _component.TendencyToArgue);
            Assert.NotNull(_component.TendencyToDefend);
            Assert.Less(0, _component.TendencyToDefend);
            Assert.NotNull(_component.TendencyToExplain);
            Assert.Less(0, _component.TendencyToExplain);
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
            _component = (SpeechComponent)_factory.Person(new Coord()).GetComponent<SpeechComponent>();
            _answer = _component.GetDetails();
            Assert.Less(2, _answer.Length);
        }
    }
}
