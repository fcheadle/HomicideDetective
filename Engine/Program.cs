using Engine.Components;
using Engine.Components.Creature;
using Engine.Extensions;
using Engine.States;
using Microsoft.Xna.Framework;
using SadConsole;
using System;

namespace Engine
{
    public class Program
    {
        internal static CrimeSceneInvestigationState CrimeSceneInvestigation { get; private set; }
        internal static MenuState Menu { get; private set; }
        internal static GameState CurrentState { get; private set; }
        internal static BasicEntity Player { get => CurrentState.Map.ControlledGameObject; }
        private static BasicMap debugOriginalMap;
        internal static void Main()
        {
            SadConsole.Game.Create(Settings.GameWidth, Settings.GameHeight);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }
        private static void Update(GameTime time)
        {
            CrimeSceneInvestigation.BlowWind(time.ElapsedGameTime);
        }

        private static void Init()
        {
            CrimeSceneInvestigation = new CrimeSceneInvestigationState(Settings.MapWidth, Settings.MapHeight, Settings.GameWidth, Settings.GameHeight);
            Global.CurrentScreen = CrimeSceneInvestigation;
            CurrentState = CrimeSceneInvestigation;
        }

        public static void Start()
        {
            Main();
        }
    }
}
