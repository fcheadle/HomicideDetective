using System;
using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using HomicideDetective.Places;
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
        public static Weather Weather = null!;
        
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
            Map = GenerateMap();
            PlayerCharacter = GeneratePlayerCharacter();
            Map.AddEntity(PlayerCharacter);
            Map.AllComponents.Add(new SurfaceComponentFollowTarget { Target = PlayerCharacter });
            Page = GenerateMessageWindow();
            Map.Children.Add(Page.BackgroundSurface);
            
            GenerateMystery();
            
            GameHost.Instance.Screen = Map;
        }

        private static PageComponent<Thoughts> GenerateMessageWindow()
        {            
             var thoughts = PlayerCharacter.AllComponents.GetFirst<Thoughts>();
             var page = new PageComponent<Thoughts>(thoughts);
             page.BackgroundSurface.Position = (Width - page.TextSurface.Surface.Width - 1, 0);
             page.BackgroundSurface.IsVisible = true;
             return page;
        }

        private static void GenerateMystery()
        {
            Mystery mystery = new Mystery(1,1);
            mystery.Generate();

            var murderWeapon = mystery.MurderWeapon;
            murderWeapon.Position = RandomFreeSpace();
            Map.AddEntity(murderWeapon);

            var murderer = mystery.Murderer;
            murderer.Position = RandomFreeSpace();
            Map.AddEntity(murderer);

            var victim = mystery.Victim;
            victim.Position = RandomFreeSpace();
            Map.AddEntity(victim);

            foreach (var witness in mystery.Witnesses)
            {
                witness.Position = RandomFreeSpace();
                Map.AddEntity(witness);
            }
            

            var thoughts = PlayerCharacter.AllComponents.GetFirst<Thoughts>();
            thoughts.Think(mystery.SceneOfCrime.Details);
            Page.Print();
        }

        private static Point RandomFreeSpace()
        {
            var point = Map.WalkabilityView.RandomPosition();
            while(Map.GetEntitiesAt<RogueLikeEntity>(point).Any() || !Map.WalkabilityView[point])
                point = Map.WalkabilityView.RandomPosition();
            
            return point;
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

            Weather = new Weather(map);
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