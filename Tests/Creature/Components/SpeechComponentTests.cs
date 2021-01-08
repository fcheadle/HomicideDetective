// using HomicideDetective.Old.Creatures;
// using HomicideDetective.Old.Creatures.Components;
// using GoRogue;
// using NUnit.Framework;
//
// namespace Tests.Creature.Components
// {
//     class SpeechComponentTests : TestBase
//     {
//         SpeechComponent _component;
//         string[] _answer;
//         DefaultCreatureFactory _factory = new DefaultCreatureFactory();
//         [SetUp]
//         public void SetUp()
//         {
//             _component = (SpeechComponent)_factory.Person(new Coord()).GetComponent<SpeechComponent>();
//             _answer = _component.GetDetails();
//         }
//
//         [Test]
//         public void NewSpeechComponentTest()
//         {
//             Assert.IsNotNull(_component);
//             Assert.NotNull(_component);
//             Assert.NotNull(_component.TendencyToMinimize);
//             Assert.Less(0, _component.TendencyToMinimize);
//             Assert.NotNull(_component.TendencyToInvalidate);
//             Assert.Less(0, _component.TendencyToInvalidate);
//             Assert.NotNull(_component.TendencyToDeny);
//             Assert.Less(0, _component.TendencyToDeny);
//             Assert.NotNull(_component.TendencyToJustify);
//             Assert.Less(0, _component.TendencyToJustify);
//             Assert.NotNull(_component.TendencyToArgue);
//             Assert.Less(0, _component.TendencyToArgue);
//             Assert.NotNull(_component.TendencyToDefend);
//             Assert.Less(0, _component.TendencyToDefend);
//             Assert.NotNull(_component.TendencyToExplain);
//             Assert.Less(0, _component.TendencyToExplain);
//         }
//         [Test]
//         public void GetDetailsTest()
//         {
//             Assert.Less(2, _answer.Length);
//         }
//     }
// }
