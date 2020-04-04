using Engine.Creatures;
using Engine.Maps;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

namespace Engine.Interface
{
    internal class UI : ContainerConsole
    {
        private readonly double _radius;

        public TerrainMap Map { get; }
        public ScrollingConsole MapRenderer { get; }
        public MessageConsole Thoughts { get; }
        public MessageConsole Messages { get; }
        public MessageConsole Dialogue { get; }


        public UI(int mapWidth, int mapHeight, int viewportWidth, int viewportHeight, Radius radius)
        {
            
            Map = GenerateMap(mapWidth, mapHeight);
            Thoughts = new MessageConsole(24, viewportHeight / 3 - 2);
            Thoughts.Position = new Coord(viewportWidth - 25, 1);
            //Thoughts.FillWithRandomGarbage();
            Messages = new MessageConsole(24, viewportHeight / 3 - 2);
            Messages.Position = new Coord(viewportWidth - 25, viewportHeight / 3);
            //Messages.FillWithRandomGarbage();
            Dialogue = new MessageConsole(24, viewportHeight / 3 - 2);
            Dialogue.Position = new Coord(viewportWidth - 25, viewportHeight / 3 * 2 - 1);
            //Dialogue.FillWithRandomGarbage();
            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, viewportWidth - 26, viewportHeight), Global.FontDefault);
            Children.Add(MapRenderer);
            Children.Add(Thoughts);
            Children.Add(Messages);
            Children.Add(Dialogue);
            Map.ControlledGameObject.IsFocused = true; // Set player to receive input, since in this example the player handles movement

            // Set up to recalculate FOV and set camera position appropriately when the player moves.  Also make sure we hook the new
            // Player if that object is reassigned.
            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObjectChanged;

            // Calculate initial FOV and center camera
            Map.CalculateFOV(Map.ControlledGameObject.Position, Map.ControlledGameObject.FOVRadius, radius);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }
        private static TerrainMap GenerateMap(int width, int height)
        {
            //TerrainMap generation happens in the constructor
            var map = new TerrainMap(width, height);

            Coord posToSpawn;
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
            Map.CalculateFOV(Map.ControlledGameObject.Position, Map.ControlledGameObject.FOVRadius, Settings.FOVRadius);
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
