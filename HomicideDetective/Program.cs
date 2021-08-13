using HomicideDetective.UserInterface;
using SadConsole;

namespace HomicideDetective
{
    class Program
    {
        //game window properties
        public const int Width = 80;
        public const int Height = 25;
     
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
            CurrentGame = new GameContainer();            
            GameHost.Instance.Screen = CurrentGame;
            CurrentGame.PlayerCharacter.Position = (0, 0);
        }
    }
}