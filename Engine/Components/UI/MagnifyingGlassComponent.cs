using Engine.Utilities;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components.UI
{
    public class MagnifyingGlassComponent : Component
    {
        const int _width = 3;
        const int _height = 3;
        public DrawingSurface Surface;
        public GameActions Purpose;
        public MagnifyingGlassComponent(BasicEntity parent, Coord position, GameActions purpose): base(false, true, true, true)
        {
            Parent = parent;
            Surface = new DrawingSurface(3, 3);
            Surface.Position = new Coord(-1, -1);
            Surface.OnDraw = (ds) =>
            {
                ds.Surface.Effects.UpdateEffects(Global.GameTimeElapsedUpdate);
                ds.Surface.Fill(Color.Gold, Color.Transparent, 0);
                ds.Surface.SetGlyph(0, 1, '(');
                ds.Surface.SetGlyph(2, 1, ')');
                ds.Surface.SetGlyph(2, 2, '\\');
            };
            Surface.MouseMove += MoveWithMouse;
            Surface.MouseButtonClicked += MouseButtonClicked;
            Surface.IsFocused = true;
        }

        private void MouseButtonClicked(object sender, MouseEventArgs mouse)
        {
            //Take action on person/place/thing?

            //destroy the magnifying glass
            Parent.Components.Remove(this);
        }

        private void MoveWithMouse(object sender, MouseEventArgs mouse)
        {
            Surface.Position = mouse.MouseState.CellPosition - new Coord(1,1);
        }

        public override string[] GetDetails()
        {
            return new string[]
            {
                "Just a magnifying glass.",
                "You shouldn't be seeing this message."
            };
        }

        public override void ProcessTimeUnit()
        {
            //do nothing?
        }
    }
}
