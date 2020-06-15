using Engine.Components;
using Engine.Components.Creature;
using Engine.Entities.Creatures;
using Engine.Entities.Items;
using Engine.Entities.Terrain;
using Engine.Extensions;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using SadConsole;
using System;

namespace Engine
{
    public class Program
    {
        public static Game CurrentGame { get; private set; }
        internal static void Main()
        {

        }
        public static void Start(
            Settings settings = null,
            ICreatureFactory creatureFactory = null, 
            IItemFactory itemFactory = null, 
            ITerrainFactory terrainFactory = null
            )
        {
            settings = settings ?? new Settings();
            creatureFactory = creatureFactory ?? new CreatureFactory();
            terrainFactory = terrainFactory ?? new TerrainFactory();
            itemFactory = itemFactory ?? new MockItemFactory();
            CurrentGame = new Game(settings, creatureFactory, itemFactory, terrainFactory);
            CurrentGame.Start();
            CurrentGame.Stop();
        }
    }
}
