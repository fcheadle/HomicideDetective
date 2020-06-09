using GoRogue;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Color = Microsoft.Xna.Framework.Color;
using Console = SadConsole.Console;

namespace Engine.Components.UI
{
    public class PageComponent<T> : Component, IDisplay where T : Component
    {
        const int _width = 24;
        const int _height = 24;
        private string[] _content = { };
        public Window Window { get; }
        public Button MaximizeButton { get; }
        private PaperWindowTheme _theme;
        internal T Component;
        DrawingSurface _surface;
        bool _hasDrawn;
        public PageComponent(BasicEntity parent, Coord position) : base(true, false, true, true)
        {
            Parent = parent;
            Component = (T)Parent.GetConsoleComponent<T>();
            Name = "Display for " + Component.Name;
            _theme = new PaperWindowTheme();
            Window = new Window(_width, _height)
            {
                DefaultBackground = Color.Tan,
                Title = Component.Name,
                TitleAlignment = HorizontalAlignment.Center,
                IsVisible = true,
                IsFocused = false,
                FocusOnMouseClick = false,
                Position = position,
                ViewPort = new GoRogue.Rectangle(0, 0, _width, _height),
                CanTabToNextConsole = true,
                Theme = _theme,
                ThemeColors = ThemeColor.Paper
            };
            Window.ThemeColors.RebuildAppearances();
            Window.MouseButtonClicked += MinimizeMaximize;

            int width = Window.Title.Length + 2; //* 3 
            int height = 3;
            List<Cell> buttonCells = new List<Cell>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //set the button text
                    int glyph;
                    if (i != 0 && i != 2) //not the top or bottom row
                    {
                        if (j != 0 && j != width - 1)
                        {
                            glyph = Component.Name[j - 1];
                        }
                        else
                            glyph = '>';
                    }
                    else
                        glyph = '_';


                    Cell here = new Cell(ThemeColor.Paper.Text, ThemeColor.Paper.ControlBack, glyph);
                    buttonCells.Add(here);
                }
            }
            MaximizeButton = new Button(Name.Length + 2, 3)
            {
                Theme = new PaperButtonTheme(),
                ThemeColors = ThemeColor.Paper,
                IsVisible = false,
                Text = Window.Title,
                TextAlignment = HorizontalAlignment.Center,
                Surface = new CellSurface(width, 3, buttonCells.ToArray())
            };
            MaximizeButton.MouseButtonClicked += MaximizeButtonClicked;
            _surface = new DrawingSurface(_width - 2, _height - 2);
            _surface.Position = new Coord(1, 1);
            _surface.OnDraw = (surface) =>
            {
                _surface.Surface.Effects.UpdateEffects(Global.GameTimeElapsedUpdate);

                if (_hasDrawn) return;

                _surface.Surface.Fill(Color.Blue, Color.Tan, '_');
                int i = 0;
                foreach (string text in Component.GetDetails())
                {
                    _surface.Surface.Print(0, i, text);
                    i++; 
                }
                _hasDrawn = true;
            };
            
            Window.Add(_surface);
        }

        private void MaximizeButtonClicked(object sender, MouseEventArgs e)
        {
            Window.Show();
            MaximizeButton.IsVisible = true;
        }
        public void Print(string[] text)
        {
            Window.Remove(_surface);

            _surface.Surface.Fill(Color.Blue, Color.Tan, '_');
            int i = 0;
            foreach (string line in text)
            {
                _surface.Surface.Print(0, i, line);
                i++;
            }
            Window.Add(_surface);
        }
        public override void Draw(Console console, TimeSpan delta)
        {
            base.Draw(console, delta);
        }

        public override void Update(Console console, TimeSpan delta)
        {
            Print(Component.GetDetails());
            base.Update(console, delta);
        }

        public void MinimizeMaximize(object sender, MouseEventArgs args)
        {
            if (args.MouseState.Mouse.RightClicked)
            {
                if (Window.IsVisible)
                    Window.Hide();
                else
                    Window.Show();


                MaximizeButton.IsVisible = !MaximizeButton.IsVisible;
                MaximizeButton.Surface.IsDirty = true;
            }
        }
        public override string[] GetDetails()
        {
            return Component.GetDetails();
        }

        internal void Print()
        {
            Print(_content);
        }

        public override void ProcessTimeUnit()
        {
            Print(Component.GetDetails());
        }
    }
}
