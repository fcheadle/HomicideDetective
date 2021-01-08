using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using Color = Microsoft.Xna.Framework.Color;
using Console = SadConsole.Console;

namespace Engine.UI.Components
{
    //refactor this for clarity upon alpha
    public class PageComponent<T> : ComponentBase, IDisplay where T : ComponentBase
    {
        const int Width = 24;
        const int Height = 24;

        private string[] _content = { };
        public Window Window { get; private set; }
        public Coord Position { get; private set; }
        public Button MaximizeButton { get; private set; }
        public T Component { get; }
        DrawingSurface _backgroundSurface;
        ScrollingConsole _textSurface;
        
        public PageComponent(BasicEntity parent, Coord position) : base(true, false, true, true)
        {
            Parent = parent;
            Position = position;
            Component = (T) parent.GetConsoleComponent<T>();
            Name = "Display for " + Component.Name;

            InitWindow();
            InitButton();
            InitBackground();
            InitTextSurface();
        }

        private void InitWindow()
        {
            Window = new Window(Width, Height)
            {
                DefaultBackground = Color.Tan,
                Title = Component.Name,
                TitleAlignment = HorizontalAlignment.Center,
                IsVisible = false,
                IsFocused = false,
                FocusOnMouseClick = false,
                Position = Position,
                ViewPort = new Rectangle(0, 0, Width, Height),
                CanTabToNextConsole = true,
                Theme = new PaperWindowTheme(),
                ThemeColors = ThemeColors.Paper
            };
            Window.ThemeColors.RebuildAppearances();
            Window.MouseButtonClicked += MinimizeMaximize;
        }

        private void InitBackground()
        {
            _backgroundSurface = new DrawingSurface(Width - 2, Height - 2);
            _backgroundSurface.Position = new Coord(1, 1);
            _backgroundSurface.Surface.Fill(Color.Blue, Color.Tan, '_');
            _backgroundSurface.OnDraw = (surface) => { }; //do nothing
            Window.Add(_backgroundSurface);
        }

        public void Elaborate(string[] content)
        {
            _content = content;
        }

        private void InitButton()
        {
            MaximizeButton = new Button(Window.Title.Length + 2, 3)
            {
                Theme = new PaperButtonTheme(),
                ThemeColors = ThemeColors.Paper,
                IsVisible = true,
                Text = Window.Title,
                TextAlignment = HorizontalAlignment.Center,
                Surface = new CellSurface(Window.Title.Length + 2, 3, ButtonCellArray(Window.Title))
            };
            MaximizeButton.MouseButtonClicked += MaximizeButtonClicked;
            //Game.UIManager.Controls.Add(MaximizeButton);
        }

        private Cell[] ButtonCellArray(string title)
        {
            int width = title.Length + 2; //* 3 
            int height = 3;
            List<Cell> buttonCells = new List<Cell>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //set the button text
                    int glyph;
                    if (i == 0)//top row
                    {
                        if (j == width - 1)
                            glyph = 191;
                        else
                            glyph = 196;
                    }
                    else
                    {
                        if (j != 0 && j != width - 1)//neither top nor bottom row
                            glyph = Component.Name[j - 1];
                        else if (j == 0)
                            glyph = ' ';
                        else
                            glyph = 179;
                    }

                    Cell here = new Cell(ThemeColors.Paper.Text, ThemeColors.Paper.ControlBack, glyph);
                    buttonCells.Add(here);
                }
            }
            return buttonCells.ToArray();
        }

        private void InitTextSurface()
        {
            _textSurface = new ScrollingConsole(Width - 2, Height - 2) 
            {
                UsePrintProcessor = true,
                Position = new Coord(1, 1),
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.Blue,
            };
            
            _textSurface.Cursor.UseStringParser = true;
            _textSurface.Cursor.Position = new Coord(0, 0);
            foreach (string detail in GetDetails())
            {
                ColoredString text = new ColoredString(detail, Color.Blue, Color.Transparent);
                _textSurface.Cursor.Print(text);
                _textSurface.Cursor.NewLine();
            }

            Window.Children.Add(_textSurface);
        }

        private void MaximizeButtonClicked(object sender, MouseEventArgs e)
        {
            if (Window.IsVisible)
                Window.Hide();
            else
                Window.Show();
            Game.UiManager.Player.IsFocused = true;
        }
        public void Print(string[] text)
        {
            
            Window.Children.Remove(_textSurface);
            InitTextSurface();
        }

        public override void Update(Console console, TimeSpan delta)
        {
            Print(GetDetails());
            if (Window.IsFocused) Game.UiManager.Player.IsFocused = true;
            if (MaximizeButton.IsFocused) Game.UiManager.Player.IsFocused = true;
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
            }
        }
        public override string[] GetDetails()
        {
            return _content.Concat(Component.GetDetails()).ToArray();
        }

        internal void Print()
        {
            Print(GetDetails());
        }

        public override void ProcessTimeUnit()
        {
        }
    }
}
