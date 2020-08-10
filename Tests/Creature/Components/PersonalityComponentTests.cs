using Engine.Creatures.Components;
using NUnit.Framework;

namespace Tests.Creature.Components
{
    class PersonalityComponentTests : TestBase
    {
        PersonalityComponent _component;
        string[] _answer;

        [SetUp]
        public void SetUp()
        {
            _component = (PersonalityComponent)Game.Player.GetComponent<PersonalityComponent>();
        }
        [Test]
        public void NewPersonalityComponentTest()
        {
            Assert.NotNull(_component);
            Assert.NotNull(_component.Passion);
            Assert.Less(0, _component.Passion);
            Assert.NotNull(_component.Ambition);
            Assert.Less(0, _component.Ambition);
            Assert.NotNull(_component.Courage);
            Assert.Less(0, _component.Courage);
            Assert.NotNull(_component.Creativity);
            Assert.Less(0, _component.Creativity);
            Assert.NotNull(_component.Empathy);
            Assert.Less(0, _component.Empathy);
            Assert.NotNull(_component.Adventurousness);
            Assert.Less(0, _component.Adventurousness);
            Assert.NotNull(_component.Spirituality);
            Assert.Less(0, _component.Spirituality);
            Assert.NotNull(_component.Laziness);
            Assert.Less(0, _component.Laziness);
            Assert.NotNull(_component.Jealousness);
            Assert.Less(0, _component.Jealousness);

            Assert.NotNull(_component.Lustfulness);
            Assert.Less(0, _component.Lustfulness);
            Assert.NotNull(_component.Greediness);
            Assert.Less(0, _component.Greediness);
            Assert.NotNull(_component.ProclivityToAnger);
            Assert.Less(0, _component.ProclivityToAnger);
            Assert.NotNull(_component.Pridefulness);
            Assert.Less(0, _component.Pridefulness);
            Assert.NotNull(_component.Sadism);
            Assert.Less(0, _component.Sadism);
            Assert.NotNull(_component.NeedToControl);
            Assert.Less(0, _component.NeedToControl);

            Assert.NotNull(_component.ImportanceOfFamily);
            Assert.Less(0, _component.ImportanceOfFamily);
            Assert.NotNull(_component.ImportanceOfFriendship);
            Assert.Less(0, _component.ImportanceOfFriendship);
            Assert.NotNull(_component.ImportanceOfBodilyHealth);
            Assert.Less(0, _component.ImportanceOfBodilyHealth);
            Assert.NotNull(_component.ImportanceOfWealth);
            Assert.Less(0, _component.ImportanceOfWealth);
            Assert.NotNull(_component.ImportanceOfReligion);
            Assert.Less(0, _component.ImportanceOfReligion);
            Assert.NotNull(_component.WorkEthic);
            Assert.Less(0, _component.WorkEthic);
            Assert.NotNull(_component.AttentionToDetail);
            Assert.Less(0, _component.AttentionToDetail);
        }

        [Test]
        public void GetDetailsTest()
        {
            _answer = _component.GetDetails();
            Assert.Less(20, _answer.Length);
        }
    }
}
