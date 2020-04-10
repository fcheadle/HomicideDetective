namespace Engine
{
    internal class Program
    {
        public static UI.GameScreen MapScreen { get; set; }

        public static void Main()
        {
            SadConsole.Game.Create(Settings.GameWidth, Settings.GameHeight);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            MapScreen = new UI.GameScreen(Settings.MapWidth, Settings.MapHeight, Settings.GameWidth, Settings.GameHeight, Settings.FOVRadius);
            SadConsole.Global.CurrentScreen = MapScreen;
        }
    }
}
