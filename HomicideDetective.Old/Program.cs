using HomicideDetective.Old.Creatures;
using HomicideDetective.Old.Items;
using HomicideDetective.Old.Scenes.Terrain;

namespace HomicideDetective.Old
{
    public class Program
    {
        public static Game CurrentGame { get; private set; }
        internal static void Main()
        {
            Start();
        }
        public static void Start(
            Settings settings = null,
            ICreatureFactory creatureFactory = null, 
            IItemFactory itemFactory = null, 
            DefaultTerrainFactory terrainFactory = null
            )
        {
            settings = settings ?? new Settings();
            creatureFactory = creatureFactory ?? new DefaultCreatureFactory();
            terrainFactory = terrainFactory ?? new DefaultTerrainFactory();
            itemFactory = itemFactory ?? new DefaultItemFactory();
            CurrentGame = new Game(settings, creatureFactory, itemFactory, terrainFactory);
            CurrentGame.Start();
            CurrentGame.Stop();
        }
    }
}
