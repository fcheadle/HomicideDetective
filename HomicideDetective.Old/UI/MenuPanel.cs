using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using System;
using HomicideDetective.Old.UI.Components;

namespace HomicideDetective.Old.UI
{
    public class MenuPanel : ControlsConsole
    {
        readonly Coord _xIncrement = new Coord(Game.Settings.GameWidth / 6, 0);
        readonly Coord _yIncrement = new Coord(0, Game.Settings.GameHeight / 6);
        public readonly BasicEntity Selector;
        public MenuPanel(int width, int height, Coord position = new Coord()) : base(width, height)
        {
            Theme = new MenuControlsTheme();
            ThemeColors = UI.ThemeColors.Menu;
            Position = position;
            Selector = new BasicEntity(Color.White, Color.Black, 16, default, 0, true, true);
            Components.Add(new MenuKeyboardComponent(this));
        }
        public void Arrange()
        {
            int i = 0;
            foreach (var button in Controls)
            {
                button.IsVisible = true;
                button.IsEnabled = true;
                button.Position = new Coord(0, 2 + (i * 2));
                i++;
            }
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);
            if (IsFocused && IsVisible)
                Selector.IsFocused = true;
        }
        public void SlideLeft()
        {
            Position -= _xIncrement;
        }

        public void SlideRight()
        {
            Position += _xIncrement;
        }

        public void SlideUp()
        {
            Position -= _yIncrement;
        }

        public void SlideDown()
        {
            Position += _yIncrement;
        }

        public void FadeIn()
        {

        }

        public void FaceOut()
        {

        }
        internal void SelectControl(int buttonIndex)
        {
            ((Button)Controls[buttonIndex]).DoClick();
        }
    }
}
