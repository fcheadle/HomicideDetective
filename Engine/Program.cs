using Engine.Maps;
using Engine.Interface;
using System;

namespace Engine
{
    internal class Program
    {
        private static bool _isPaused { get; set; } = false;
        public static UI MapScreen { get; set; }

        public static void Main()
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(Settings.GameWidth, Settings.GameHeight);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            MapScreen = new UI(Settings.MapWidth, Settings.MapHeight, Settings.GameWidth, Settings.GameHeight, Settings.FOVRadius);
            SadConsole.Global.CurrentScreen = MapScreen;
        }

        internal static void TogglePause()
        {
            
        }
    }
}
