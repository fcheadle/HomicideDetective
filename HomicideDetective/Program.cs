using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.Mysteries;
using HomicideDetective.People;
using HomicideDetective.Places;
using HomicideDetective.UserInterface;
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
        public static PageComponent<Thoughts> Page = null!;
        public static Mystery Mystery = null!;
        
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
            Page = GenerateMessageWindow();
            Mystery = GenerateMystery();
            
            GameHost.Instance.Screen = Map;
        }

        private static RogueLikeMap GenerateMap()
        {
            //use GoRogue generator to combine the steps in Places/Generation and get the resulting three maps
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
            
            //combine the three smaller maps into one real map
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

            //add additional components
            Weather = new Weather(map);
            
            return map;
        }

        private static RogueLikeEntity GeneratePlayerCharacter()
        {
            //creation
            var position = Map.WalkabilityView.RandomPosition();
            var player = new RogueLikeEntity(position, 1, false)
            {
                IsFocused = true,
            };
            
            //general personhood
            player.AllComponents.Add(new Thoughts());
            player.AllComponents.Add(new Speech());
            
            //player controls
            var motionControl = new PlayerControlsComponent();
            motionControl.AddKeyCommand(Keys.T, KeyCommands.Talk);
            motionControl.AddKeyCommand(Keys.L, KeyCommands.Look);
            player.AllComponents.Add(motionControl);
            
            //add to map and make view center on player
            Map.AddEntity(player);
            Map.AllComponents.Add(new SurfaceComponentFollowTarget { Target = player });
            
            return player;
        }

        private static PageComponent<Thoughts> GenerateMessageWindow()
        {
            //create the component and feed it the player's current thoughts
            var thoughts = PlayerCharacter.AllComponents.GetFirst<Thoughts>();
            var page = new PageComponent<Thoughts>(thoughts);
            
            //size the background surface to perfection
            page.BackgroundSurface.Position = (Width - page.TextSurface.Surface.Width - 1, 0);
            page.BackgroundSurface.IsVisible = true;

            //add it to the map
            Map.Children.Add(page.BackgroundSurface);
            
            return page;
        }

        private static Mystery GenerateMystery()
        {
            //create the mystery
            Mystery mystery = new Mystery(1,1);
            mystery.Generate();

            //put the murder weapon on the map
            var murderWeapon = mystery.MurderWeapon;
            murderWeapon.Position = RandomFreeSpace();
            Map.AddEntity(murderWeapon);

            //put the murderer on the map
            var murderer = mystery.Murderer;
            murderer.Position = RandomFreeSpace();
            Map.AddEntity(murderer);

            //add victim's corpse to the map
            var victim = mystery.Victim;
            victim.Position = RandomFreeSpace();
            Map.AddEntity(victim);

            //populate the map with witnesses
            foreach (var witness in mystery.Witnesses)
            {
                witness.Position = RandomFreeSpace();
                Map.AddEntity(witness);
            }
            
            //think thoughts about the crime scene
            var thoughts = PlayerCharacter.AllComponents.GetFirst<Thoughts>();
            thoughts.Think(mystery.SceneOfCrime.Details);
            Page.Print();
            return mystery;
        }

        private static Point RandomFreeSpace()
        {
            var point = Map.WalkabilityView.RandomPosition();
            while(Map.GetEntitiesAt<RogueLikeEntity>(point).Any() || !Map.WalkabilityView[point])
                point = Map.WalkabilityView.RandomPosition();
            
            return point;
        }
    }
}