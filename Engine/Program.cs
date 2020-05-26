using Engine.Components;
using Engine.Components.Creature;
using Engine.Extensions;
using Engine.UI;
using Microsoft.Xna.Framework;
using SadConsole;
using System;

namespace Engine
{
    public class Program
    {
        internal static CrimeSceneInvestigationState CrimeSceneInvestigation { get; private set; }
        internal static DebuggingState Debug { get; private set; }
        internal static MenuState Menu { get; private set; }
        internal static GameState CurrentState { get; private set; }
        internal static TimeSpan ActCounter { get; private set; } = TimeSpan.FromSeconds(0);
        private static BasicMap debugOriginalMap;
        internal static void Main()
        {
            SadConsole.Game.Create(Settings.GameWidth, Settings.GameHeight);
            //SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnInitialize = DebugInit;
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void DebugInit()
        {
            Debug = new DebuggingState();
            Global.CurrentScreen = Debug;
            CurrentState = Debug;
            SadConsole.Game.OnUpdate = DebugUpdate;
            debugOriginalMap = Debug.Map;
        }

        private static void DebugUpdate(GameTime time)
        {
            Debug.Player.GetGoRogueComponent<KeyboardComponent>().ProcessGameFrame(); //doesnt work for some reason?

            BasicMap rotated = debugOriginalMap.Rotate(Debug.Player.Position, 33, (int)time.ElapsedGameTime.TotalSeconds);
            Debug.Map.ReplaceTiles(rotated, new GoRogue.Coord(0, 0));
            Debug.MapRenderer.IsDirty = true;
        }

        private static void Update(GameTime time)
        {
            ActCounter += time.ElapsedGameTime;
            CrimeSceneInvestigation.BlowWind(time.ElapsedGameTime);
            CrimeSceneInvestigation.Player.GetGoRogueComponent<KeyboardComponent>().ProcessGameFrame();
            CrimeSceneInvestigation.MapRenderer.IsDirty = true;// (obj.ElapsedGameTime);
            if (ActCounter > TimeSpan.FromMilliseconds(250))
            {
                foreach (SadConsole.BasicEntity creature in CrimeSceneInvestigation.Map.GetCreatures())
                {
                    creature.GetGoRogueComponent<ActorComponent>().Act();
                }
                ActCounter = TimeSpan.FromMilliseconds(0);
            }
        }

        private static void Init()
        {
            SadConsole.Game.OnUpdate = Update;
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
