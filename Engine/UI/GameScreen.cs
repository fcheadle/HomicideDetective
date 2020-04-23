using Engine.Creatures;
using Engine.Maps;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
using XnaRect = Microsoft.Xna.Framework.Rectangle;
using TownMap = Engine.Maps.TownMap;
using System.Linq;

namespace Engine.UI
{
    internal class GameScreen : ContainerConsole
    {
        internal TownMap TownMap { get; }
        internal ScrollingConsole MapRenderer { get; }
        internal MessageConsole Messages { get; }
        internal GameScreen(int mapWidth, int mapHeight, int viewportWidth, int viewportHeight, Radius radius)
        {
            TownMap = new TownMap(mapWidth, mapHeight);
            TownMap.ControlledGameObject = new Player(TownMap.WalkabilityView.RandomPosition(true));
            TownMap.AddEntity(TownMap.ControlledGameObject);

            //Thoughts = new MessageConsole(24, viewportHeight / 3 - 2);
            //Thoughts.Position = new Coord(viewportWidth - 25, 1);
            Messages = new MessageConsole(24, viewportHeight - 2);
            Messages.Position = new Coord(viewportWidth - 25, 1);
            //Dialogue = new MessageConsole(24, viewportHeight / 3 - 2);
            //Dialogue.Position = new Coord(viewportWidth - 25, viewportHeight / 3 * 2 - 1);

            MapRenderer = TownMap.CreateRenderer(new XnaRect(0, 0, viewportWidth - 26, viewportHeight), Global.FontDefault);
            Children.Add(MapRenderer);
            //Children.Add(Thoughts);
            Children.Add(Messages);
            //Children.Add(Dialogue);
            TownMap.ControlledGameObject.IsFocused = true; // Set player to receive input, since in this example the player handles movement

            // Set up to recalculate FOV and set camera position appropriately when the player moves.  Also make sure we hook the new
            // Player if that object is reassigned.
            TownMap.ControlledGameObject.Moved += Player_Moved;
            TownMap.ControlledGameObjectChanged += ControlledGameObjectChanged;

            // Calculate initial FOV and center camera
            TownMap.CalculateFOV(TownMap.ControlledGameObject.Position, ((Player)TownMap.ControlledGameObject).FOVRadius, radius);
            MapRenderer.CenterViewPortOnPoint(TownMap.ControlledGameObject.Position);
        }
        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }
        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            TownMap.CalculateFOV(TownMap.ControlledGameObject.Position, ((Player)TownMap.ControlledGameObject).FOVRadius, Settings.FOVRadius);
            Area[] areas = ((Player)TownMap.ControlledGameObject).GetCurrentRegions().ToArray();
            if(areas!=null)
                Messages.Print(areas);

            MapRenderer.CenterViewPortOnPoint(TownMap.ControlledGameObject.Position);
        }
    }
}
