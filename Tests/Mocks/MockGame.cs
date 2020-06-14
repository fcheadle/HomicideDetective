using Engine;
using Engine.Components;
using Engine.Entities.Creatures;
using Engine.Entities.Items;
using Engine.Entities.Terrain;
using Engine.Maps;
using Engine.Utilities;
using GoRogue;
using Microsoft.Xna.Framework;
using System;
using Tests.Mocks;
using Game = Engine.Game;
using Settings = Engine.Settings;
namespace Tests
{
    class MockGame : Game
    {

        public MockGame(Action<GameTime> update) : base()
        {
            ApplySettings(new MockSettings());
            SetCreatureFactory(new MockCreatureFactory());
            SetTerrainFactory(new MockTerrainFactory());
            SetItemFactory(new MockItemFactory());
            Setup();
            SadConsole.Game.Create("font-sample.json", Settings.GameWidth, Settings.GameHeight);
            //SadConsole.Global.Fonts.Remove("IBM_16x8");
            //SadConsole.Global.Fonts.Remove("IBM_16x8_ext");
            SadConsole.Game.OnInitialize = InitializeTests;
            SadConsole.Game.OnUpdate = update;
        }

        public void RunOnce() => SadConsole.Game.Instance.RunOneFrame();

        public void SwapUpdate(Action<GameTime> action) => SadConsole.Game.OnUpdate = action;

        public void InitializeTests()
        {
            SadConsole.Global.Fonts.Remove("IBM_16x8");
            SadConsole.Global.Fonts.Remove("IBM_16x8_ext");
            CreateConsoles();
            SadConsole.Global.CurrentScreen = Container;
            Map.ControlledGameObject = CreatureFactory.Player(new Coord(15, 15));

            Player.Components.Add(new MockComponent());
        }
    }
}
