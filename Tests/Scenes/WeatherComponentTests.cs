﻿using Engine.Scenes.Terrain.Components;
using NUnit.Framework;

namespace Tests.Scenes
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
            _component = (WeatherComponent)Engine.Game.UIManager.GetComponent<WeatherComponent>();
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
            _component = (WeatherComponent)Engine.Game.UIManager.GetComponent<WeatherComponent>();
            _answer = _component.GetDetails();
            Assert.Less(2, _answer.Length);
        }

        [Test] //aka processtimeunit
        public void BlowWindTest()
        {
            _game = new MockGame(NewWeatherComponent);
            _game.RunOnce();
            _game.SwapUpdate(GetDetails);
            _game.RunOnce();
            _game.Stop();
        }
    }
}
