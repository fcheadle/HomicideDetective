using Engine.Creatures;
using Engine.Maps;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

namespace Engine.UI
{
    internal class GameScreen : ContainerConsole
    {
        internal Town Town { get; }
        internal BasicMap Map { get => Town.Map; }
        internal ScrollingConsole MapRenderer { get; }
        internal MessageConsole Thoughts { get; }
        internal MessageConsole Messages { get; }
        internal MessageConsole Dialogue { get; }
        internal GameScreen(int mapWidth, int mapHeight, int viewportWidth, int viewportHeight, Radius radius)
        {
            Town = new Town(mapWidth, mapHeight);
            Map.ControlledGameObject = new Player(Map.WalkabilityView.RandomPosition(true));
            Map.AddEntity(Map.ControlledGameObject);

            Thoughts = new MessageConsole(24, viewportHeight / 3 - 2);
            Thoughts.Position = new Coord(viewportWidth - 25, 1);
            Messages = new MessageConsole(24, viewportHeight / 3 - 2);
            Messages.Position = new Coord(viewportWidth - 25, viewportHeight / 3);
            Dialogue = new MessageConsole(24, viewportHeight / 3 - 2);
            Dialogue.Position = new Coord(viewportWidth - 25, viewportHeight / 3 * 2 - 1);

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
            Map.CalculateFOV(Map.ControlledGameObject.Position, ((Player)Map.ControlledGameObject).FOVRadius, radius);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }
        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }
        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            Map.CalculateFOV(Map.ControlledGameObject.Position, ((Player)Map.ControlledGameObject).FOVRadius, Settings.FOVRadius);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }
    }
}
