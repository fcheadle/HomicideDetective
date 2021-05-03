using System.Collections.Generic;
using GoRogue.MapGeneration;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Integration.Maps;

namespace HomicideDetective
{
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        public const int MapWidth = 100;
        public const int MapHeight = 60;

        // Initialized in Init, so null-override is used.
        public static RogueLikeMap Map = null!;
        public static RogueLikeEntity PlayerCharacter = null!;
        public static ScreenSurface MessageWindow = null!;
        static void Main(/*string[] args*/)
        {
            Game.Create(Width, Height);
            Game.Instance.OnStart = Init;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }
        
        private static void Init()
        {
            // Generate map
            Map = GenerateMap();

            // Generate player and add to map
            PlayerCharacter = GeneratePlayerCharacter();
            Map.AddEntity(PlayerCharacter);
            
            foreach(RogueLikeEntity entity in GenerateMystery())
                Map.AddEntity(entity);
            
            // Center view on player
            Map.AllComponents.Add(new SurfaceComponentFollowTarget { Target = PlayerCharacter });
            
            MessageWindow = GenerateMessageWindow();
            Map.Children.Add(MessageWindow);
            
            GameHost.Instance.Screen = Map;
        }

        private static ScreenSurface GenerateMessageWindow()
        {            
             var thoughts = PlayerCharacter.AllComponents.GetFirst<Thoughts>();
             thoughts.Think("Good Luck, detective.");
             var page = new PageComponent<Thoughts>(thoughts);
             page.Page.Position = (Width - page.Page.Surface.Width, 0);
             page.Page.IsVisible = true;
             PlayerCharacter.AllComponents.Add(page);
             page.Print();
             return page.Page;
        }

        private static IEnumerable<RogueLikeEntity> GenerateMystery()
        {
            Mystery mystery = new Mystery(1,1);
            var murderWeapon = new RogueLikeEntity(Map.WalkabilityView.RandomPosition(), 'm', false, true, 1);
            murderWeapon.AllComponents.Add(mystery.GenerateMurderWeapon());
            yield return murderWeapon;
            
            var murderer = new RogueLikeEntity(Map.WalkabilityView.RandomPosition(), 1, false);
            murderer.AllComponents.Add(mystery.GeneratePerson("Thornton"));
            murderer.AllComponents.Add(new Thoughts());
            murderer.AllComponents.Add(new Speech());
            yield return murderer;
            
            var victim = new RogueLikeEntity(Map.WalkabilityView.RandomPosition(), 2, false);
            var victimInfo = mystery.GenerateVictim("Reed");
            victim.AllComponents.Add(victimInfo);
            yield return victim;
        }

        private static RogueLikeMap GenerateMap()
        {
            var generator = new Generator(MapWidth, MapHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(new Places.Generation.GrassStep());
                    gen.AddSteps(new Places.Generation.StreetStep(-25));
                    gen.AddSteps(new Places.Generation.HouseStep(-25));
                });

            var grassMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            var streetMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("street");
            var houseMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("house");

            RogueLikeMap map = new RogueLikeMap(MapWidth, MapHeight, 4, Distance.Euclidean, viewSize: (Width * 2 / 3 + 1, Height));

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
            // player.AllComponents.Add(GeneratePersonalInfo());
            player.AllComponents.Add(new Thoughts());
            var motionControl = new PlayerControlsComponent();
            motionControl.AddKeyCommand(Keys.T, Talk);
            motionControl.AddKeyCommand(Keys.L, Look);
            player.AllComponents.Add(motionControl);
            player.IsFocused = true;
            return player;
        }

        private static void Talk()
        {
            var thoughts = PlayerCharacter.AllComponents.GetFirst<Thoughts>();
            for (int i = PlayerCharacter.Position.X - 1; i < PlayerCharacter.Position.X + 2; i++)
            {
                for (int j = PlayerCharacter.Position.Y - 1; j < PlayerCharacter.Position.Y + 2; j++)
                {
                    if (Map.Contains((i, j)))
                    {
                        foreach (var entity in Map.GetEntitiesAt<RogueLikeEntity>((i,j)))
                        {
                            var speech = entity.AllComponents.GetFirstOrDefault<Speech>();
                            if(speech is not null)
                                thoughts.Think(speech.Details.ToArray());
                        }
                    }
                }
            }

            var page = PlayerCharacter.AllComponents.GetFirst<PageComponent<Thoughts>>();
            page.Print();
        }

        private static void Look()
        {
            var thoughts = PlayerCharacter.AllComponents.GetFirst<Thoughts>();
            for (int i = PlayerCharacter.Position.X - 1; i < PlayerCharacter.Position.X + 2; i++)
            {
                for (int j = PlayerCharacter.Position.Y - 1; j < PlayerCharacter.Position.Y + 2; j++)
                {
                    if (Map.Contains((i, j)) && PlayerCharacter.Position != (i,j))
                    {
                        foreach (var entity in Map.GetEntitiesAt<RogueLikeEntity>((i,j)))
                        {
                            var substantive = entity.AllComponents.GetFirstOrDefault<Substantive>();
                            if(substantive is not null)
                                thoughts.Think(substantive.Details);
                        }
                    }
                }
            }

            var page = PlayerCharacter.AllComponents.GetFirst<PageComponent<Thoughts>>();
            page.Print();
        }
    }
}

//
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
//             //_messageWindow = GenerateMessageWindow();
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
//             page.Page.Position = (Width - page.Page.Surface.Width, 0);
//             page.Page.IsVisible = true;
//             _playerCharacter.AllComponents.Add(page);
//
//             _map.Children.Add(page.Page);
//             return page.Page;
//         }
//     }
// }