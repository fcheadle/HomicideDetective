using System;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using HomicideDetective.Places.Weather;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;

namespace HomicideDetective.UserInterface
{
    public class GameContainer : ScreenObject
    {
        public RogueLikeMap Map { get; set; }
        public MessageWindow MessageWindow { get; set; }
        public DateTime CurrentTime { get; set; }

        //game window properties
        public const int Width = 80;
        public const int Height = 25;
        
        //map properties
        public const int MapWidth = 100;
        public const int MapHeight = 60;
        //public Weather Weather = null!;

        //ui and game properties
        public RogueLikeEntity PlayerCharacter;
        public Mystery Mystery;

        public GameContainer()
        {
            Mystery = new Mystery(1, 1);
            Mystery.Generate(MapWidth, MapHeight, Width * 2 / 3, Height);
            Map = InitMap();
            PlayerCharacter = InitPlayerCharacter();
            MessageWindow = InitMessageWindow();
            MessageWindow.Write(Mystery.CurrentPlaceInfo(PlayerCharacter.Position));
            CurrentTime = DateTime.Now;//todo
            //Weather = new Weather(Mystery.CurrentLocation);
            GameHost.Instance.FrameUpdate += (s, e) => Map.AllComponents.GetFirst<WeatherController>().Animate();
        }

        private RogueLikeMap InitMap()
        {
            var map = Mystery.CurrentLocation;
            Children.Add(map);
            return map;
        }
        private RogueLikeEntity InitPlayerCharacter()
        {
            //creation
            var position = Mystery.RandomFreeSpace(Map);
            var player = new RogueLikeEntity(position, 1, false)
            {
                IsFocused = true,
            };
            
            //general personhood
            var thoughts = new Memories();
            player.AllComponents.Add(thoughts);
            player.AllComponents.Add(new Speech());
            
            //player controls
            var motionControl = InitKeyCommands();
            player.AllComponents.Add(motionControl);
            
            //add to map and make view center on player
            Map.AddEntity(player);
            Map.DefaultRenderer?.SadComponents.Add(new SurfaceComponentFollowTarget { Target = player });

            player.PositionChanged += ProcessUnitOfTime;
            player.PositionChanged += (e, s) =>
            {
                MessageWindow.Clear();
                MessageWindow.Write(Mystery.CurrentPlaceInfo(PlayerCharacter.Position));
                Mystery.CurrentLocation.PlayerFOV.Calculate(PlayerCharacter.Position, 12, Mystery.CurrentLocation.DistanceMeasurement);
            };
            return player;
        }
        private PlayerControlsComponent InitKeyCommands()
        {
            var motionControl = new PlayerControlsComponent();
            motionControl.AddKeyCommand(Keys.T, KeyCommands.Talk);
            motionControl.AddKeyCommand(Keys.L, KeyCommands.Look);
            motionControl.AddKeyCommand(Keys.I, KeyCommands.Inspect);
            motionControl.AddKeyCommand(Keys.M, KeyCommands.NextMap);
            motionControl.AddKeyCommand(Keys.PageUp, MessageWindow.BackPage);
            motionControl.AddKeyCommand(Keys.PageDown, MessageWindow.ForwardPage);
            motionControl.AddKeyCommand(Keys.Insert, MessageWindow.WriteGarbage); //for testing purposes
            motionControl.AddKeyCommand(Keys.Home, MessageWindow.UpOneLine);
            motionControl.AddKeyCommand(Keys.End, MessageWindow.DownOneLine);
            return motionControl;
        }
        private MessageWindow InitMessageWindow()
        {
            //create the component and feed it the player's current thoughts
            var page = new MessageWindow(Width / 3 + 1, Height);
            
            //size the background surface to perfection
            page.BackgroundSurface.Position = (Width - page.TextSurface.Surface.Width - 1, 0);
            page.BackgroundSurface.IsVisible = true;

            Children.Add(page.BackgroundSurface);
            return page;
        }
        public static void ProcessUnitOfTime(object? sender, ValueChangedEventArgs<Point> valueChangedEventArgs)
        {
            //todo
        }

        public void NextMap()
        {
            Children.Remove(Map);
            Map.RemoveEntity(PlayerCharacter);
            Mystery.NextMap();
            Map = Mystery.CurrentLocation;
            PlayerCharacter.Position = Mystery.RandomFreeSpace(Map);
            if(!Map.Entities.Contains(PlayerCharacter))
                Map.AddEntity(PlayerCharacter);
            
            Map.DefaultRenderer?.SadComponents.Add(new SurfaceComponentFollowTarget { Target = PlayerCharacter });
            //Weather = new Weather(Map);
            Children.Add(Map);
        }
    }
}