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
        private Window LookingGlass;
        public DrawingSurface Surface;
        public GameAction Purpose;
        public MagnifyingGlassComponent(BasicEntity parent, Coord position, GameAction purpose = GameAction.LookAtEverythingInSquare): base(false, true, true, true)
        {
            Parent = parent;
            LookingGlass = new Window(3, 3);
            CellSurface surface = new CellSurface(3, 3);
            //surface.Position = new Coord(-1, -1);
            //surface.OnDraw = (ds) =>
            //{
            //    ds.Surface.Effects.UpdateEffects(Global.GameTimeElapsedUpdate);
            //    ds.Surface.Fill(Color.Gold, Color.Transparent, 0);
            //    ds.Surface.SetGlyph(0, 1, '(');
            //    ds.Surface.SetGlyph(2, 1, ')');
            //    ds.Surface.SetGlyph(2, 2, '\\');
            //};
            //surface.MouseMove += MoveWithMouse;
            //surface.MouseButtonClicked += MouseButtonClicked;
            //surface.IsFocused = true;
            //surface.IsVisible = true;
            //Game.Container.Children.Add(surface);
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
            Surface.IsVisible = true;
            Surface.IsFocused = true;
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
