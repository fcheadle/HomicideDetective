using HomicideDetective.UserInterface;
using SadConsole;

namespace HomicideDetective
{
    class Program
    {
        //game window properties
        public const int Width = 120;
        public const int Height = 40;
     
        public static GameContainer CurrentGame = null!;
        
        static void Main()
        {
            Game.Create(Width, Height);
            Game.Instance.OnStart = Init;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            CurrentGame = new GameContainer {UseKeyboard = true};            
            GameHost.Instance.Screen = CurrentGame;
        }
    }
}