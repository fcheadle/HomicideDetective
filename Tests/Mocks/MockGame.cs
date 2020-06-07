using Engine;
using Engine.Entities.Creatures;
using Engine.Entities.Items;
using Engine.Entities.Terrain;
using Engine.States;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using Tests.Mocks;
using Game = Engine.Game;
using Settings = Engine.Settings;
namespace Tests
{
    class MockGame : IGame
    {
        public static MockState DebugState { get; private set; }
        public static MenuState Menu { get; private set; }
        public static BasicEntity Player { get; private set; }

        public ISettings Settings { get; } = new MockSettings();
        public ICreatureFactory CreatureFactory { get; } = new MockCreatureFactory();
        public ITerrainFactory TerrainFactory { get; } = new MockTerrainFactory();
        public IItemFactory ItemFactory { get; } = new MockItemFactory();

        public MockGame(Action<GameTime> update)
        {
            SadConsole.Game.Create("font-sample.json", Settings.GameWidth, Settings.GameHeight);
            SadConsole.Global.Fonts.Remove("IBM_16x8");
            SadConsole.Global.Fonts.Remove("IBM_16x8_ext");
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = update;
        }
        public void Start() => SadConsole.Game.Instance.Run();

        public void RunOnce() => SadConsole.Game.Instance.RunOneFrame();

        public void SwapUpdate(Action<GameTime> action) => SadConsole.Game.OnUpdate = action;

        public void Update(GameTime time) => throw new NotImplementedException();

        public void Stop()
        {
            SadConsole.Game.Instance.Exit();
            SadConsole.Game.Instance.Dispose();
        }
        public void Init()
        {
            Global.Fonts.Remove("IBM_16x8");// = new Dictionary<string, FontMaster>();
            Global.Fonts.Remove("IBM_16x8_ext");// = new Dictionary<string, FontMaster>();
            DebugState = new MockState();
            Global.CurrentScreen = DebugState;
            Program.RegisterTestGame(this, Settings, CreatureFactory, ItemFactory, TerrainFactory);
            Player = DebugState.Map.ControlledGameObject;
            Player.Components.Add(new MockComponent());
        }
    }
}
