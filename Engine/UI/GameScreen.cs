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
        internal GameScreen(int mapWidth, int mapHeight, int viewportWidth, int viewportHeight)
        {
            TownMap = new TownMap(mapWidth, mapHeight);
            TownMap.ControlledGameObject = new Player(new Coord(33,33));
            TownMap.AddEntity(TownMap.ControlledGameObject);
            
            Messages = new MessageConsole(24, viewportHeight);
            Messages.Position = new Coord(viewportWidth - 24, 0);

            MapRenderer = TownMap.CreateRenderer(new XnaRect(0, 0, viewportWidth - 25, viewportHeight), Global.FontDefault);
            Children.Add(MapRenderer);
            Children.Add(Messages);
            
            TownMap.ControlledGameObject.IsFocused = true; 
            TownMap.ControlledGameObject.Moved += Player_Moved;
            TownMap.ControlledGameObjectChanged += ControlledGameObjectChanged;

            TownMap.CalculateFOV(TownMap.ControlledGameObject.Position, ((Player)TownMap.ControlledGameObject).FOVRadius);
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
            if (areas != null)
                Messages.Print(areas);

            MapRenderer.CenterViewPortOnPoint(TownMap.ControlledGameObject.Position);
        }
    }
}