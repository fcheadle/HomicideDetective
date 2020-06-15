using Engine.Maps;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Entities;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Entities
{
    public class MagnifyingGlass : Entity
    {
        public MagnifyingGlass(Coord position) : base(3,3)
        {
            Position = position;

            //this section straight out of 
            Animation = new AnimatedConsole("Looking Glass", 3, 3, Global.FontDefault);
            Animation.SetSurface(PrintingCells());
            animation.CreateFrame();
            Animations = new Dictionary<string, AnimatedConsole>();
            Animations.Add("looking glass", animation);
            MouseMove += Mouse_Move;
        }

        private CellSurface PrintingCells()
        {
            CellSurface surface = new CellSurface(3, 3);
            surface.DefaultBackground = Color.Transparent;
            surface.DefaultForeground = Color.DarkGoldenrod;
            surface.SetGlyph(0, 0, ' ');   surface.SetGlyph(1, 0, ' '); surface.SetGlyph(2, 0, ' ');
            surface.SetGlyph(0, 1, '(');   surface.SetGlyph(1, 1, ' '); surface.SetGlyph(2, 1, ')');
            surface.SetGlyph(0, 2, ' ');   surface.SetGlyph(1, 2, ' '); surface.SetGlyph(2, 2, '\\');
            return surface;
        }

        private void Mouse_Move(object sender, MouseEventArgs e)
        {
            Position = (Coord)e.MouseState.CellPosition - 1;
        }
    }
}
