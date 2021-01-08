using HomicideDetective.Old;
using HomicideDetective.Old.Creatures;
using HomicideDetective.Old.Items;
using HomicideDetective.Old.Scenes.Terrain;

namespace HomicideDetective
{
    //need to reduce the memory footprint of this significantly
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            Settings settings = new Settings();
            DefaultCreatureFactory creatureFactory = new DefaultCreatureFactory();
            IItemFactory itemFactory = new DefaultItemFactory();
            DefaultTerrainFactory terrainFactory = new DefaultTerrainFactory();
            Old.Program.Start(settings, creatureFactory,itemFactory,terrainFactory);
        }
    }
}
