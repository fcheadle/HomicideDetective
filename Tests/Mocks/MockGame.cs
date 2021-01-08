using System;
using HomicideDetective.Old.Creatures;
using HomicideDetective.Old.Items;
using HomicideDetective.Old.Scenes.Components;
using HomicideDetective.Old.Scenes.Terrain;
using Microsoft.Xna.Framework;
using Game = HomicideDetective.Old.Game;
using Settings = HomicideDetective.Old.Settings;

namespace Tests.Mocks
{
    public class MockGame : Game
    {
        public MockGame(Action<GameTime> update)
        {
            ApplySettings(new Settings());
            Settings.GameWidth = 64;
            Settings.GameHeight = 64;
            Settings.MapWidth = 128;
            Settings.MapHeight = 128;

            SetCreatureFactory(new DefaultCreatureFactory());
            SetTerrainFactory(new DefaultTerrainFactory());
            SetItemFactory(new DefaultItemFactory());
            SadConsole.Game.Create("font-sample.json", Settings.GameWidth, Settings.GameHeight);
            SadConsole.Game.OnInitialize = InitializeTests;
            SadConsole.Game.OnUpdate = update;
        }

        public void RunOnce() => SadConsole.Game.Instance.RunOneFrame();

        public void SwapUpdate(Action<GameTime> action) => SadConsole.Game.OnUpdate = action;

        public void InitializeTests()
        {
            base.Init();
            SadConsole.Global.Fonts.Remove("IBM_16x8");
            SadConsole.Global.Fonts.Remove("IBM_16x8_ext");
            UiManager.Components.Add(new WeatherComponent());
            Player.Components.Add(new MockComponent());
        }
    }
}
