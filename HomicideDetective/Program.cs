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
        public static PageComponent<Thoughts> Page = null!;
        public static Mystery Mystery = null!;
        static void Main(/*string[] args*/)
        {
            Game.Create(Width, Height);
            Game.Instance.OnStart = Init;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }
        
        private static void Init()
        {
            Map = GenerateMap();
            PlayerCharacter = GeneratePlayerCharacter();
            Map.AddEntity(PlayerCharacter);
            Map.AllComponents.Add(new SurfaceComponentFollowTarget { Target = PlayerCharacter });
            Page = GenerateMessageWindow();
            Map.Children.Add(Page.BackgroundSurface);
            
            foreach(RogueLikeEntity entity in GenerateMystery())
                Map.AddEntity(entity);
            
            GameHost.Instance.Screen = Map;
        }

        private static PageComponent<Thoughts> GenerateMessageWindow()
        {            
             var thoughts = PlayerCharacter.AllComponents.GetFirst<Thoughts>();
             var page = new PageComponent<Thoughts>(thoughts);
             page.BackgroundSurface.Position = (Width - page.TextSurface.Surface.Width - 1, 0);
             page.BackgroundSurface.IsVisible = true;
             // PlayerCharacter.AllComponents.Add(page);
             return page;
        }

        private static IEnumerable<RogueLikeEntity> GenerateMystery()
        {
            Mystery mystery = new Mystery(1,1);
            mystery.Generate();
            
            var murderWeapon = new RogueLikeEntity(Map.WalkabilityView.RandomPosition(), (char)mystery.MurderWeapon.Name![0], false);
            murderWeapon.AllComponents.Add(mystery.MurderWeapon);
            yield return murderWeapon;
            
            var murderer = new RogueLikeEntity(Map.WalkabilityView.RandomPosition(), 1, false);
            murderer.AllComponents.Add(mystery.Murderer);
            murderer.AllComponents.Add(new Thoughts());
            murderer.AllComponents.Add(new Speech());
            yield return murderer;
            
            var victim = new RogueLikeEntity(Map.WalkabilityView.RandomPosition(), 2, false);
            victim.AllComponents.Add(mystery.Victim);
            yield return victim;

            foreach(var witnessDetails in mystery.Witnesses)
            {
                var witness = new RogueLikeEntity(Map.WalkabilityView.RandomPosition(), 1, false);
                witness.AllComponents.Add(witnessDetails);
                witness.AllComponents.Add(new Thoughts());
                witness.AllComponents.Add(new Speech());
                yield return witness;
            }

            var thoughts = PlayerCharacter.AllComponents.GetFirst<Thoughts>();
            thoughts.Think(mystery.SceneOfCrime.Details);
            Page.Print();
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
            var position = Map.WalkabilityView.RandomPosition();
            var player = new RogueLikeEntity(position, 1, false);
            player.AllComponents.Add(new Thoughts());
            player.AllComponents.Add(new Speech());
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
                    if (Map.Contains((i, j)) && (i,j) != PlayerCharacter.Position)
                    {
                        foreach (var entity in Map.GetEntitiesAt<RogueLikeEntity>((i,j)))
                        {
                            var speech = entity.AllComponents.GetFirstOrDefault<Speech>();
                            if (speech is not null)
                            {
                                thoughts.Think(speech.Details.ToArray());
                                Page.Print();
                            }
                        }
                    }
                }
            }

            Page.Print();
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

            Page.Print();
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