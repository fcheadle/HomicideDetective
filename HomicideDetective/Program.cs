using System;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using HomicideDetective.Places;
using HomicideDetective.UserInterface;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;
using SadRogue.Primitives;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Integration.Maps;

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
            Game.Instance.FrameUpdate += (s,e) => CurrentGame.Weather.ProcessTimeUnit();
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            CurrentGame = new GameContainer();            
            GameHost.Instance.Screen = CurrentGame;
        }
    }
}