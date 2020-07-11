namespace HomicideDetective
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Settings settings = new Engine.Settings();
            Engine.Creatures.DefaultCreatureFactory creatureFactory = new Engine.Creatures.DefaultCreatureFactory();
            Engine.Items.IItemFactory itemFactory = new Engine.Items.DefaultItemFactory();
            Engine.Scenes.Terrain.DefaultTerrainFactory terrainFactory = new Engine.Scenes.Terrain.DefaultTerrainFactory();
            
            
            Engine.Program.Start(settings, creatureFactory,itemFactory,terrainFactory);
        }
    }
}
