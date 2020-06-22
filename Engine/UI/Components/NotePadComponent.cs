using Engine.Scenes.Areas;
using Engine.UI;
using Engine.Utilities;
using GoRogue;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;
using Console = SadConsole.Console;

namespace Engine.Components.UI
{
    //Everything is working in this class,
    //except scrolling back and forth
    public class NotePadComponent : Component, IDisplay
    {
        public Window Window { get; private set; }
        public Button MaximizeButton { get; private set; }
        public Button BackPageButton { get; private set; }
        public Button NextPageButton { get; private set; }
        public TextArea Text { get; private set; }
        public ScrollingConsole Surface { get; }
        private DrawingSurface _backgroundSurface;
        const int _width = 32;
        const int _height = 32;
        const int _maxLines = 100 * _height;
        public int PageNumber = 0;

        public NotePadComponent(BasicEntity parent, Coord position) : base(true, true, false, true)
        {
            Parent = parent;
            Name = "Notepad";
            InitWindow();
            InitBackground();
            InitNavigationButtons();
            InitMaximizeButton();

            Surface = new ScrollingConsole(_width - 2, _height - 2)
            {
                UseKeyboard = false,
                IsVisible = true,
                Position = new Coord(1, 1),
                DefaultBackground = Color.Transparent,
                DefaultForeground = Color.Blue
            };
            Window.Children.Add(Surface);
            InitTextSurface();
            Window.Position = position;
        }

        #region take notes
        public override string[] GetDetails()
        {
            return new string[] { Text.Text };
        }

        public void WriteLine(string line)
        {
            Text.WriteLine(line);
        }
        #endregion

        #region init
        private void InitWindow()
        {
            Window = new Window(_width, _height)
            {
                DefaultBackground = Color.Tan,
                Title = Name,
                TitleAlignment = HorizontalAlignment.Center,
                IsVisible = false,
                IsFocused = false,
                FocusOnMouseClick = true,
                ViewPort = new GoRogue.Rectangle(0, 0, _width, _height),
                Theme = new PaperWindowTheme(),
                ThemeColors = ThemeColors.Paper,
                CanTabToNextConsole = true,
            };
            Window.MouseButtonClicked += MouseButton_Clicked;
        }

        private void InitTextSurface()
        {
            Text = new TextArea(_width - 2, _maxLines);
            Text.Position = new Coord(1, 1);
            Window.Add(Text);
        }

        private void InitNavigationButtons()
        {
            BackPageButton = new Button(2) { Position = new Coord(0, _height - 1), Text = "<=", };
            BackPageButton.Theme = new PaperButtonTheme();
            BackPageButton.ThemeColors = ThemeColors.Paper;
            BackPageButton.ThemeColors.Appearance_ControlOver.Foreground = Color.Black;
            BackPageButton.ThemeColors.Appearance_ControlOver.Background = Color.Tan;
            BackPageButton.ThemeColors.ControlBack = Color.Transparent;
            BackPageButton.ThemeColors.ControlHostBack = Color.Transparent;
            BackPageButton.ThemeColors.ControlHostFore = Color.Blue;
            BackPageButton.MouseButtonClicked += BackButton_Clicked;
            Window.Add(BackPageButton);

            NextPageButton = new Button(2) { Position = new Coord(_width - 2, _height - 1), Text = "=>" };
            NextPageButton.Theme = new PaperButtonTheme();
            NextPageButton.ThemeColors = ThemeColors.Paper;
            NextPageButton.ThemeColors.Appearance_ControlOver.Foreground = Color.Black;
            NextPageButton.ThemeColors.Appearance_ControlOver.Background = Color.Tan;
            NextPageButton.ThemeColors.ControlBack = Color.Transparent;
            NextPageButton.ThemeColors.ControlHostBack = Color.Transparent;
            NextPageButton.ThemeColors.ControlHostFore = Color.Blue; 
            NextPageButton.MouseButtonClicked += NextButton_Clicked;
            Window.Add(NextPageButton);
        }
        private void InitMaximizeButton()
        {
            int width = Window.Title.Length + 2; //* 3 
            int height = 3;
            List<Cell> buttonCells = new List<Cell>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //set the button text
                    int glyph;
                    if (i == 0) //not the top or bottom row
                    {
                        if (j == width - 1)
                            glyph = 191;
                        else
                            glyph = 196;
                    }
                    else
                    {
                        if (j != 0 && j != width - 1)
                            glyph = Name[j - 1];
                        else if (j == 0)
                            glyph = ' ';
                        else
                            glyph = 179;
                    }


                    Cell here = new Cell(ThemeColors.Paper.Text, ThemeColors.Paper.ControlBack, glyph);
                    buttonCells.Add(here);
                }
            }

            MaximizeButton = new Button(Name.Length + 2, 3)
            {
                Theme = new PaperButtonTheme(),
                ThemeColors = ThemeColors.Paper,
                IsVisible = true,
                Text = Window.Title,
                TextAlignment = HorizontalAlignment.Center,
                Surface = new CellSurface(width, 3, buttonCells.ToArray())
            };
            MaximizeButton.MouseButtonClicked += MaximizeButton_Clicked;
        }

        private void InitBackground()
        {
            _backgroundSurface = new DrawingSurface(_width - 2, _height - 2);
            _backgroundSurface.Position = new Coord(1, 1);
            _backgroundSurface.Surface.Fill(Color.Blue, Color.Tan, '_');
            _backgroundSurface.OnDraw = (surface) => { }; //do nothing
            Window.Add(_backgroundSurface);
        }
        #endregion

        private void MaximizeButton_Clicked(object sender, MouseEventArgs e)
        {
            Toggle();
        }

        private void Toggle()
        {
            if (Window.IsVisible)
            {
                Window.Hide();
                Game.UIManager.ControlledGameObject.IsFocused = true;
            }
            else
            {
                Window.Show();
                Window.IsFocused = true;
            }
        }

        public void MouseButton_Clicked(object sender, MouseEventArgs args)
        {
            if (args.MouseState.Mouse.RightClicked)
            {
                Toggle();
            }
        }

        public override void Draw(Console console, TimeSpan delta)
        {
            Window.Draw(delta);
            base.Draw(console, delta);
        }

        public override void Update(Console console, TimeSpan delta)
        {
            if (Window.IsFocused)
                Text.IsFocused = true;

            Window.Update(delta);
            Text.IsDirty = true;
            Surface.Clear();
            Surface.Cursor.Position = new Coord(0, 0);
            ColoredString text = new ColoredString(Text.Text, Color.Black, Color.Transparent);
            Surface.Cursor.Print(text);

            base.Update(console, delta);
        }

        public override void ProcessMouse(Console console, MouseConsoleState state, out bool handled)
        {
            Window.ProcessMouse(state);
            base.ProcessMouse(console, state, out handled);
        }

        public override void ProcessKeyboard(Console console, SadConsole.Input.Keyboard state, out bool handled)
        {
            Window.ProcessKeyboard(state);
            Text.ProcessKeyboard(state);
            //if(state.IsKeyPressed(Game.Settings.KeyBindings[GameAction.RefocusOnPlayer])) Game.
            handled = true;
            //base.ProcessKeyboard(console, state, out handled);
        }

        private void NextButton_Clicked(object sender, MouseEventArgs e)
        {
            PageNumber++;
            Surface.ViewPort = new Rectangle(0, PageNumber * _height, _width - 2, _height - 2);
        }

        private void BackButton_Clicked(object sender, MouseEventArgs e)
        {
            PageNumber--;
            Surface.ViewPort = new Rectangle(0, PageNumber * _height, _width - 2, _height - 2);
        }

        public override void ProcessTimeUnit()
        {
        }
    }
}
