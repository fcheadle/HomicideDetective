using System.Linq;
using GoRogue.MapGeneration;
using HomicideDetective.New;
using HomicideDetective.New.Scenes.Generation;
using HomicideDetective.New.UserInterface;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;

namespace ExampleGame
{
    /// <summary>
    /// A tiny game to give examples of how to use GoRogue
    /// </summary>
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        private const int MapWidth = 80;// for now
        private const int MapHeight = 25; // for now
        private static RogueLikeMap Map;
        private static RogueLikeEntity PlayerCharacter;
        private static SettableCellSurface MapWindow;
        private static ScreenSurface MessageWindow;
        
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
            Map = GenerateMap();
            PlayerCharacter = GeneratePlayerCharacter();
            GameHost.Instance.Screen = Map.CreateRenderer();
            MessageWindow = GenerateMessageWindow();
        }

        private static RogueLikeMap GenerateMap()
        {
            var generator = new Generator(MapWidth, MapHeight)
                .AddStep(new ColorfulGrassStep())
                .AddStep(new BlockGenerationStep())
                .AddStep(new HouseGenerationStep())
                .Generate();
            
            RogueLikeMap map = new RogueLikeMap(MapWidth, MapHeight, 4, Distance.Euclidean);
            
            
            var generatedMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("grass");
            foreach(var location in generatedMap.Positions())
                map.SetTerrain(generatedMap[location]);
            
            generatedMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("block");
            foreach(var location in generatedMap.Positions().Where(p=> generatedMap[p] != null))
                map.SetTerrain(generatedMap[location]);

            generatedMap = generator.Context.GetFirst<ISettableGridView<RogueLikeCell>>("house");
            foreach(var location in generatedMap.Positions().Where(p => generatedMap[p] != null))
                map.SetTerrain(generatedMap[location]);
            
            var cells = new ArrayView<ColoredGlyph>(MapWidth, MapHeight);
            cells.ApplyOverlay(map.TerrainView);

            MapWindow = new SettableCellSurface(map, Width, Height);
            
            return map;
        }

        private static RogueLikeEntity GeneratePlayerCharacter()
        {
            var position = Map.WalkabilityView.Positions().First(p => Map.WalkabilityView[p]);
            var player = new RogueLikeEntity(position, 1, false, true, 1);

            player.AddComponent(new PlayerControlsComponent());
            player.IsFocused = true;
            Map.AddEntity(player);

            return player;
        }

        private static ScreenSurface GenerateMessageWindow()
        {
            var health = new HealthComponent();
            var page = new PageComponent<HealthComponent>(health);
            
            PlayerCharacter.AddComponent(health);
            PlayerCharacter.AddComponent(page);

            GameHost.Instance.Screen.Children.Add(page.Window);
            return page.Window;
        }
    }
}