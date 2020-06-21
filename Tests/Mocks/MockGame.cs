using Engine.Entities.Terrain;
using Engine.Items;
using Engine.Scenes.Components;
using Microsoft.Xna.Framework;
using System;
using Game = Engine.Game;
using Settings = Engine.Settings;

namespace Tests
{
    class MockGame : Game
    {
        public MockGame(Action<GameTime> update) : base()
        {
            ApplySettings(new Settings());
            SetCreatureFactory(new MockCreatureFactory());
            SetTerrainFactory(new MockTerrainFactory());
            SetItemFactory(new DefaultItemFactory());
            SadConsole.Game.Create("font-sample.json", Settings.GameWidth, Settings.GameHeight);
            SadConsole.Game.OnInitialize = InitializeTests;
            SadConsole.Game.OnUpdate = update;
        }

        public void RunOnce() => SadConsole.Game.Instance.RunOneFrame();

        public void SwapUpdate(Action<GameTime> action) => SadConsole.Game.OnUpdate = action;

        public void InitializeTests()
        {
            SadConsole.Global.Fonts.Remove("IBM_16x8");
            SadConsole.Global.Fonts.Remove("IBM_16x8_ext");
            base.Init();
            UIManager.Components.Add(new WeatherComponent());
            Player.Components.Add(new MockComponent());
        }
    }
}
