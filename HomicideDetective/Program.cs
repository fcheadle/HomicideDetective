using System.Linq;
using GoRogue.MapGeneration;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Integration.Maps;

namespace HomicideDetective
{
    /// <summary>
    /// A tiny game to give examples of how to use GoRogue
    /// </summary>
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        public const int MapWidth = 100;
        public const int MapHeight = 60;

        // Initialized in Init, so null-override is used.
        public static RogueLikeMap Map = null!;
        public static RogueLikeEntity PlayerCharacter = null!;
        static void Main(/*string[] args*/)
        {
            Game.Create(Width, Height);
            Game.Instance.OnStart = Init;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        /// <summary>
        /// Runs before the game starts
        /// </summary>
        private static void Init()
        {
            // Generate map
            Map = GenerateMap();

            // Generate player and add to map
            PlayerCharacter = GeneratePlayerCharacter();
            Map.AddEntity(PlayerCharacter);

            // Center view on player
            Map.AllComponents.Add(new SadConsole.Components.SurfaceComponentFollowTarget { Target = PlayerCharacter });

            GameHost.Instance.Screen = Map;
        }

        private static RogueLikeMap GenerateMap()
        {
            // Generate a rectangular map for the sake of testing.
            var generator = new Generator(MapWidth, MapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new HomicideDetective.Places.Generation.GrassStep());
                    gen.AddSteps(new HomicideDetective.Places.Generation.StreetStep(-25));
                    gen.AddSteps(new HomicideDetective.Places.Generation.HouseStep(-25));
                });

            var grassMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            var streetMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("street");
            var houseMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("house");

            RogueLikeMap map = new RogueLikeMap(MapWidth, MapHeight, 4, Distance.Euclidean, viewSize: (Width, Height));

            foreach(var location in map.Positions())
            {
                if(houseMap[location] != null)
                    map.SetTerrain(houseMap[location]);
                
                else if(streetMap[location] != null) 
                    map.SetTerrain(streetMap[location]);
                
                else
                    map.SetTerrain(grassMap[location]);

            }

            return map;
        }

        private static RogueLikeEntity GeneratePlayerCharacter()
        {
            //var position = Map.WalkabilityView.Positions().First(p => Map.WalkabilityView[p]);
            var position = (Map.Width / 2, Map.Height / 2);
            var player = new RogueLikeEntity(position, 1, false);

            var motionControl = new PlayerControlsComponent();
            player.AllComponents.Add(motionControl);
            player.IsFocused = true;
            return player;
        }
    }
}


// using System.Collections.Generic;
// using System.Linq;
// using GoRogue.Random;
// using HomicideDetective.Mysteries;
// using HomicideDetective.People;
// using SadConsole;
// using SadConsole.Input;
// using SadRogue.Primitives.GridViews;
// using SadRogue.Integration;
// using SadRogue.Integration.Components;
// using SadRogue.Integration.Maps;
//
// namespace HomicideDetective
// {
//     class Program
//     {
//         public const int Width = 80;
//         public const int Height = 25;
//         public const int MapWidth = 80;// for now
//         public const int MapHeight = 25; // for now
//         
//         private static Mystery CurrentMystery;
//         private static List<Mystery> _mysteries;
//         private static RogueLikeMap _map;
//         private static RogueLikeEntity _playerCharacter;
//         private static ScreenSurface _messageWindow;
//         static void Main()
//         {
//             Game.Create(Width, Height);
//             Game.Instance.OnStart = Init;
//             Game.Instance.Run();
//             Game.Instance.Dispose();
//         }
//
//         /// <summary>
//         /// Runs before the game starts
//         /// </summary>
//         private static void Init()
//         {
//             CurrentMystery = new Mystery(GlobalRandom.DefaultRNG.Next(), 1);
//             CurrentMystery.CommitMurder();
//             CurrentMystery.Open();
//             _map = CurrentMystery.CurrentScene;
//             _playerCharacter = GeneratePlayerCharacter();
//             GameHost.Instance.Screen = _map;
//             _messageWindow = GenerateMessageWindow();
//         }
//         
//         private static RogueLikeEntity GeneratePlayerCharacter()
//         {
//             var position = _map.WalkabilityView.Positions().First(p => _map.WalkabilityView[p]);
//             var player = new Person(position, new Substantive());
//             
//             var controls = new PlayerControlsComponent();
//             
//             controls.AddKeyCommand(Keys.Left, player.InteractLeft);
//             controls.AddKeyCommand(Keys.Right, player.InteractRight);
//             controls.AddKeyCommand(Keys.Up, player.InteractUp);
//             controls.AddKeyCommand(Keys.Down, player.InteractDown);
//             
//             player.AllComponents.Add(controls);
//             player.IsFocused = true;
//             _map.AddEntity(player);
//
//             return player;
//         }
//
//         private static ScreenSurface GenerateMessageWindow()
//         {
//             var thoughts = _playerCharacter.AllComponents.GetFirst<Thoughts>();
//             thoughts.Think($"Case number 1, {CurrentMystery.Victim.Name}.");
//             var page = new PageComponent<Thoughts>(thoughts);
//             page.Window.Position = (Width - page.Window.Surface.Width, 0);
//             page.Window.IsVisible = true;
//             _playerCharacter.AllComponents.Add(page);
//
//             _map.Children.Add(page.Window);
//             return page.Window;
//         }
//     }
// }