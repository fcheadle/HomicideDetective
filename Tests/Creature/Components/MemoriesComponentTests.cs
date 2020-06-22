using Engine.Creatures;
using Engine.Creatures.Components;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Creature.Components
{
    class MemoriesComponentTests : TestBase
    {
        EmotionsComponent _component;
        string[] _answer;
        CreatureFactory _factory = new CreatureFactory();
        [SetUp]
        public void SetUp()
        {
            _component = (EmotionsComponent)_factory.Person(new Coord()).GetComponent<EmotionsComponent>();
        }
        [Test]
        public void NewSpeechComponentTest()
        {
            Assert.NotNull(_component);
            Assert.AreEqual(50, _component.Angry);
            Assert.AreEqual(50, _component.Aroused);
            Assert.AreEqual(50, _component.Bored);
            Assert.AreEqual(50, _component.Confused);
            Assert.AreEqual(50, _component.Envy);
            Assert.AreEqual(50, _component.Fear);
            Assert.AreEqual(50, _component.Grief);
            Assert.AreEqual(50, _component.Guilt);
            Assert.AreEqual(50, _component.Humiliated);
            Assert.AreEqual(50, _component.Joy);
            Assert.AreEqual(50, _component.Jealous);
            Assert.AreEqual(50, _component.Lonely);
            Assert.AreEqual(50, _component.Love);
            Assert.AreEqual(50, _component.Nervous);
            Assert.AreEqual(50, _component.Panic);
            Assert.AreEqual(50, _component.Pride);
            Assert.AreEqual(50, _component.Relief);
            Assert.AreEqual(50, _component.Sad);
            Assert.AreEqual(50, _component.Shame);
            Assert.AreEqual(50, _component.Sympathy);
        }

        [Test]
        public void GetDetailsTest()
        {
            _answer = _component.GetDetails();
            Assert.Less(2, _answer.Length);
        }
    }
}
