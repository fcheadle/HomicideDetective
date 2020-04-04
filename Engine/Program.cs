using Engine.Maps;
using Engine.Interface;
using System;

namespace Engine
{
    internal class Program
    {

        private const int StartingWidth = 160;
        private const int StartingHeight = 50;
        private static bool _isPaused;
        public static UI MapScreen { get; set; }

        public static void Main()
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(StartingWidth, StartingHeight);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            // Here we pass the viewport and map size as the same, but the map could be larger and the camera would center on the player.
            MapScreen = new UI(StartingWidth, StartingHeight, StartingWidth, StartingHeight);
            SadConsole.Global.CurrentScreen = MapScreen;
        }

        internal static void TogglePause()
        {
            
        }
    }
}
