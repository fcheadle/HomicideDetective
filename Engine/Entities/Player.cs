using System;
using System.Collections.Generic;
using Engine.Maps;
using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;

namespace Engine.Creatures
{
    internal class Player : BasicEntity
    {
        public int FOVRadius;
        internal Player(Coord position, int fovradius = 15)
            : base( Color.White, Color.Black, 1, position, 5, true, true)
        { FOVRadius = fovradius; }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            Direction moveDirection = Direction.NONE;
            foreach(Keys key in Settings.KeyBindings.Keys)
            if (info.IsKeyPressed(key))
            {
                Settings.TogglePause();
            }

            foreach (Keys key in Settings.MovementKeyBindings.Keys)
            {
                if (info.IsKeyPressed(key))
                {
                    moveDirection = Settings.MovementKeyBindings[key];
                    break;
                }
            }

            Position += moveDirection;

            if (moveDirection != Direction.NONE)
                return true;
            else
                return base.ProcessKeyboard(info);
        }

        internal IEnumerable<Area> GetCurrentRegions()
        {
            foreach(Area area in Program.MapScreen.TownMap.Regions)
            {
                if (area.InnerPoints.Contains(Position))
                    yield return area;
            }
        }
    }
}
