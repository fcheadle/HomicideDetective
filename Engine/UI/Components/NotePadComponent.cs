using Engine.Scenes.Areas;
using Engine.UI;
using GoRogue;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;
using Console = SadConsole.Console;

namespace Engine.Components.UI
{
    public class NotePadComponent : Component, IDisplay
    {
        const int _width = 32;
        const int _height = 32;
        const int _maxLines = 100 * _height;
        public Window Window { get; }
        public Button MaximizeButton {get;}
        public int PageNumber;
        public Button BackPageButton;
        public Button NextPageButton;
        private bool _hasDrawn;
        private DrawingSurface _surface;

        public NotePadComponent(BasicEntity parent, Coord position) : base(true, true, true, true)
        {
            
            Parent = parent;
            Name = "Notepad";
            Window = new Window(_width, _height)
            {
                DefaultBackground = Color.Tan,
                Title = Name,
                TitleAlignment = HorizontalAlignment.Center,
                IsVisible = false,
                IsFocused = false,
                FocusOnMouseClick = false,
                Position = position,
                ViewPort = new GoRogue.Rectangle(0, 0, _width, _height),
                Theme = new PaperWindowTheme(),
                ThemeColors = ThemeColors.Paper,
                CanTabToNextConsole = true,
            };
            BackPageButton = new Button(2) { Position = new Coord(0, _height - 1), Text = "<=",  };
            BackPageButton.MouseButtonClicked += BackButton_Clicked;
            Window.Add(BackPageButton);

            NextPageButton = new Button(2) { Position = new Coord(_width - 2, _height - 1), Text = "=>" };
            NextPageButton.MouseButtonClicked += NextButton_Clicked;
            Window.Add(NextPageButton); 
            Window.MouseButtonClicked += MinimizeMaximize;

            DrawingSurface ds = new DrawingSurface(_width - 2, _height - 2);
            ds.Position = new Coord(1, 1);
            ds.OnDraw = (surface) =>
            {
                ds.Surface.Effects.UpdateEffects(Global.GameTimeElapsedUpdate);

                if (_hasDrawn) return;

                ds.Surface.Fill(Color.Blue, Color.Tan, '_');
                //foreach(string text in )
                //ds.Surface.Print()
                _hasDrawn = true;
            };
            Window.Add(ds);

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
            MaximizeButton.MouseButtonClicked += MaximizeButtonClicked;
        }

        private void MaximizeButtonClicked(object sender, MouseEventArgs e)
        {
            if (Window.IsVisible)
                Window.Hide();
            else
                Window.Show();
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
        public void Print(string[] text)
        {
            Window.Fill(Color.Blue, Color.Tan, '_');
            for (int i = 0; i < text.Length; i++)
            {
                Window.Print(0, i, new ColoredString(text[i].ToString(), Color.DarkBlue, Color.Transparent));
            }
        }
        public void Print(Area[] areas)
        {
            for (int i = 0; i < areas.Length; i++)
            {
                Window.Print(0, i, new ColoredString(areas[i].Name, Color.DarkBlue, Color.Transparent));
            }
        }
        public override void Draw(Console console, TimeSpan delta)
        {
            Window.Draw(delta);
            base.Draw(console, delta);
        }

        public override void Update(Console console, TimeSpan delta)
        {
            Window.Update(delta);
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
            base.ProcessKeyboard(console, state, out handled);
        }
        public override string[] GetDetails()
        {
            string[] answer = { Name };
            return answer;
        }

        internal void Print()
        {

        }

        private void NextButton_Clicked(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BackButton_Clicked(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ProcessTimeUnit()
        {
        }
    }
}
