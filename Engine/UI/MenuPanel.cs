using Engine.Components.UI;
using GoRogue;
using SadConsole;

namespace Engine.UI
{
    public class MenuPanel : ControlsConsole
    {
        readonly Coord _xIncrement = new Coord(Game.Settings.GameWidth / 6, 0);
        readonly Coord _yIncrement = new Coord(0, Game.Settings.GameHeight / 6);
        public MenuPanel(int width, int height, Coord position = new Coord()) : base(width, height)
        {
            Theme = new MenuControlsTheme();
            ThemeColors = Engine.UI.ThemeColors.Menu;
            Position = position;
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
    }
}
