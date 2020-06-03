using Engine.Components.UI;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.UI
{
    public class Page : Window, IDisplay
    {
        private string _content;
        private const int _width = 32;
        private const int _height = 32;
        public ScrollingConsole Console { get; }
        public Window Window { get => this; }
        string[] _messages;
        public Page(string title, string content, Font font = null) : base(_width, _height)
        {
            Font = font ?? Global.FontDefault;
            Title = title;
            TitleAlignment = HorizontalAlignment.Center;
            _content = content;
            Position = new Coord(1,1);
            GenerateTheme();
            Console = new ScrollingConsole(_width, _height);
            DefaultBackground = Color.Tan;
            Console.Position = new Coord(0, 1);
            Console.ViewPort = new GoRogue.Rectangle(0, 0, _width, _height - 1);
            Console.Fill(Color.Blue, Color.Tan, '_');

            ViewPort = new GoRogue.Rectangle(0, 0, _width, _height);

            //mouse
            UseMouse = true;
            CanDrag = true;
            FocusOnMouseClick = true;
        }

        private void GenerateTheme()
        {
            Theme = new NotepadTheme();
            ThemeColors = new Colors()
            {
                TitleText = Color.Black,
                Lines = Color.Blue,
                TextBright = Color.LightBlue,
                Text = Color.Blue,
                TextSelected = Color.Cyan,
                TextSelectedDark = Color.DarkBlue,
                TextLight = Color.White,
                TextDark = Color.Black,
                TextFocused = Color.Blue,
                ControlBack = Color.Tan,
                ControlBackLight = Color.Khaki,
                ControlBackSelected = Color.DarkKhaki,
                ControlBackDark = Color.SaddleBrown,
                ControlHostBack = Color.Tan,
                ControlHostFore = Color.Blue,
            };

            ThemeColors.RebuildAppearances();
        }
    }
}
