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
        
        //map properties
        public const int MapWidth = 100;
        public const int MapHeight = 60;
        public static RogueLikeMap Map = null!;
        public static Weather Weather = null!;

        //ui and game properties
        public static RogueLikeEntity PlayerCharacter = null!;
        public static Page Page = null!;
        public static Mystery Mystery = null!;
        public static DateTime CurrentTime;
        
        static void Main()
        {
            Game.Create(Width, Height);
            Game.Instance.OnStart = Init;
            Game.Instance.FrameUpdate += DoWeather;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void DoWeather(object? sender, GameHost e)
        {
            Weather.ProcessTimeUnit();
        }

        private static void Init()
        {
            CurrentTime = new DateTime(1970, 7,4, 18, 00, 0);
            Mystery = new Mystery(1, 1);
            Mystery.Generate();
            Map = Mystery.GenerateMap(MapWidth, MapHeight, Width * 2 / 3 + 1, Height);
            PlayerCharacter = InitPlayerCharacter();
            Page = InitMessageWindow();
            
            GameHost.Instance.Screen = Map;
        }

        private static RogueLikeEntity InitPlayerCharacter()
        {
            //creation
            var position = Mystery.RandomFreeSpace(Map);
            var player = new RogueLikeEntity(position, 1, false)
            {
                IsFocused = true,
            };
            
            //general personhood
            var thoughts = new Thoughts();
            thoughts.Think(Mystery.SceneOfCrimeInfo.GenerateDetailedDescription());
            player.AllComponents.Add(thoughts);
            player.AllComponents.Add(new Speech());
            
            //player controls
            var motionControl = InitKeyCommands();
            player.AllComponents.Add(motionControl);
            
            //add to map and make view center on player
            Map.AddEntity(player);
            Map.AllComponents.Add(new SurfaceComponentFollowTarget { Target = player });

            player.PositionChanged += ProcessUnitOfTime;

            return player;
        }

        private static PlayerControlsComponent InitKeyCommands()
        {
            var motionControl = new PlayerControlsComponent();
            motionControl.AddKeyCommand(Keys.T, KeyCommands.Talk);
            motionControl.AddKeyCommand(Keys.L, KeyCommands.Look);
            motionControl.AddKeyCommand(Keys.PageUp, Page.BackPage);
            motionControl.AddKeyCommand(Keys.PageDown, Page.ForwardPage);
            motionControl.AddKeyCommand(Keys.Insert, Page.WriteGarbage); //for testing purposes
            motionControl.AddKeyCommand(Keys.Home, Page.UpOneLine);
            motionControl.AddKeyCommand(Keys.End, Page.DownOneLine);
            return motionControl;
        }

        private static void ProcessUnitOfTime(object? sender, ValueChangedEventArgs<Point> valueChangedEventArgs)
        {
            //todo
        }

        private static Page InitMessageWindow()
        {
            //create the component and feed it the player's current thoughts
            var page = new Page(Width / 3 + 1, Height);
            
            //size the background surface to perfection
            page.BackgroundSurface.Position = (Width - page.TextSurface.Surface.Width - 1, 0);
            page.BackgroundSurface.IsVisible = true;

            //add it to the map
            Map.Children.Add(page.BackgroundSurface);
            
            page.Write(Mystery.SceneOfCrimeInfo);

            return page;
        }
    }
}