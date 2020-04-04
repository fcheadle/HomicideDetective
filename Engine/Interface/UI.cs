using Engine.Creatures;
using Engine.Maps;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

namespace Engine.Interface
{
    internal class UI : ContainerConsole
    {
        public TerrainMap Map { get; }
        public ScrollingConsole MapRenderer { get; }
        public MessageConsole Messages { get; }
        //Generate a map and display it.  Could just as easily pass it into
        public UI(int mapWidth, int mapHeight, int viewportWidth, int viewportHeight)
        {
            Map = GenerateDungeon(mapWidth - 26, mapHeight);
            Messages = new MessageConsole(24, mapHeight - 2);
            Messages.Position = new Coord(mapWidth - 25, 1);
            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, viewportWidth, viewportHeight), Global.FontDefault);
            Children.Add(MapRenderer);
            Children.Add(Messages);
            Map.ControlledGameObject.IsFocused = true; // Set player to receive input, since in this example the player handles movement

            // Set up to recalculate FOV and set camera position appropriately when the player moves.  Also make sure we hook the new
            // Player if that object is reassigned.
            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObjectChanged;

            // Calculate initial FOV and center camera
            Map.CalculateFOV(Map.ControlledGameObject.Position, Map.ControlledGameObject.FOVRadius, Radius.SQUARE);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }
        private static TerrainMap GenerateDungeon(int width, int height)
        {
            // Same size as screen, but we set up to center the camera on the player so expanding beyond this should work fine with no other changes.
            var map = new TerrainMap(width, height);

            // Generate map via GoRogue, and update the real map with appropriate terrain.
            //var tempMap = new ArrayMap<bool>(map.Width, map.Height);
            //QuickGenerators.GenerateDungeonMazeMap(tempMap, minRooms: 10, maxRooms: 20, roomMinSize: 5, roomMaxSize: 11);
            //map.ApplyTerrainOverlay(tempMap, SpawnTerrain);

            Coord posToSpawn;
            // Spawn a few mock enemies
            for (int i = 0; i < 10; i++)
            {
                posToSpawn = map.WalkabilityView.RandomPosition(true); // Get a location that is walkable
                var goblin = new BasicEntity(Color.Red, Color.Transparent, 'g', posToSpawn, (int)MapLayer.MONSTERS, isWalkable: false, isTransparent: true);
                map.AddEntity(goblin);
            }

            // Spawn player
            posToSpawn = map.WalkabilityView.RandomPosition(true);
            map.ControlledGameObject = new Player(posToSpawn);
            map.AddEntity(map.ControlledGameObject);

            return map;
        }

        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            Map.CalculateFOV(Map.ControlledGameObject.Position, Map.ControlledGameObject.FOVRadius, Radius.SQUARE);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        private static IGameObject SpawnTerrain(Coord position, bool mapGenValue)
        {
            // Floor or wall.  This could use the Factory system, or instantiate Floor and Wall classes, or something else if you prefer;
            // this simplistic if-else is just used for example
            if (mapGenValue) // Floor
                return new BasicTerrain(Color.White, Color.Black, '.', position, isWalkable: true, isTransparent: true);
            else             // Wall
                return new BasicTerrain(Color.White, Color.Black, '#', position, isWalkable: false, isTransparent: false);
        }
    }
}
