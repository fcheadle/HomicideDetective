using Engine.Components;
using Engine.Components.Creature;
using Engine.Entities.Creatures;
using Engine.Entities.Items;
using Engine.Entities.Terrain;
using Engine.Extensions;
using Engine.States;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using SadConsole;
using System;

namespace Engine
{
    public class Program
    {
        public static MenuState Menu { get; private set; }
        public static IGame CurrentGame { get; private set; }
        public static ISettings Settings { get; private set; }
        public static ICreatureFactory CreatureFactory { get; private set; }
        public static IItemFactory ItemFactory { get; private set; }
        public static ITerrainFactory TerrainFactory { get; private set; }
        internal static void Main()
        {

        }
        public static void Start(
            IGame game = null, 
            ISettings settings = null,
            ICreatureFactory creatureFactory = null, 
            IItemFactory itemFactory = null, 
            ITerrainFactory terrainFactory = null
            )
        {
            Settings = settings ?? new Settings();
            CreatureFactory = creatureFactory ?? new CreatureFactory();
            TerrainFactory = terrainFactory ?? new TerrainFactory();
            ItemFactory = itemFactory ?? new ItemFactory();
            CurrentGame = game ?? new Game(Settings, CreatureFactory, ItemFactory, TerrainFactory);
            CurrentGame.Start();
            CurrentGame.Stop();
        }

        public static void RegisterTestGame(
            IGame game,
            ISettings settings,
            ICreatureFactory creatureFactory,
            IItemFactory itemFactory,
            ITerrainFactory terrainFactory)
        {
            CurrentGame = game;
            Settings = settings ?? new Settings();
            CreatureFactory = creatureFactory ?? new CreatureFactory();
            TerrainFactory = terrainFactory ?? new TerrainFactory();
            ItemFactory = itemFactory ?? new ItemFactory();
        }
    }
}
