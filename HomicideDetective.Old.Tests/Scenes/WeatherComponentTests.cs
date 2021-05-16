using HomicideDetective.Old.Scenes.Components;
using NUnit.Framework;

namespace HomicideDetective.Old.Tests.Scenes
{
    class WeatherComponentTests : TestBase
    {
        WeatherComponent _component;
        string[] _answer;

        [SetUp]
        public void SetUp()
        {
            _component = (WeatherComponent)HomicideDetective.Old.Game.UiManager.GetComponent<WeatherComponent>();
        }
        [Test]
        public void NewWeatherComponentTest()
        {
            Assert.NotNull(_component);
        }
        [Test]
        public void GetDetailsTest()
        {
            _answer = _component.GetDetails();
            Assert.Less(2, _answer.Length);
        }
        [Test] //aka processtimeunit
        public void BlowWindTest()
        {
            _answer = _component.GetDetails();
            Assert.Less(2, _answer.Length);
        }
    }
}
