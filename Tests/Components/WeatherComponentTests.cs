using Engine.Components;
using Engine.Components.Creature;
using Engine.Entities;
using GoRogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Components
{
    class WeatherComponentTests : TestBase
    {
        WeatherComponent _component;
        string[] _answer;

        [Test]
        public void NewWeatherComponentTest()
        {
            _game = new MockGame(NewWeatherComponent);
            _game.RunOnce();
            _game.Stop();
        }
        private void NewWeatherComponent(Microsoft.Xna.Framework.GameTime time)
        {
            _component = (WeatherComponent)MockGame.DebugState.GetComponent<WeatherComponent>();
            Assert.NotNull(_component);
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
            _component = (WeatherComponent)MockGame.DebugState.GetComponent<WeatherComponent>();
            _answer = _component.GetDetails();
            Assert.Less(2, _answer.Length);
        }
    }
}
