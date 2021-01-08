using System;
using System.Collections.Generic;
using System.Linq;
using ExampleGame.UserInterface;
using SadConsole;
using SadConsole.Input;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;
using Console = SadConsole.Console;

namespace HomicideDetective.New.UserInterface
{
    //refactor this for clarity upon alpha
    public class PageComponent<T> : RogueLikeComponentBase, IHaveDetails where T : IHaveDetails
    {
        const int Width = 24;
        const int Height = 24;

        private string[] _content = { };
        public string Name { get; }
        public string Description { get; }
        public ScreenSurface Window { get; private set; }
        public Button MaximizeButton { get; private set; }
        public T Component { get; }
        private ScreenSurface _backgroundSurface;
        private ScreenSurface _textSurface;
        
        public PageComponent(T component) : base(true, false, true, true)
        {
            Name = "PageComponent<"+nameof(T)+">";
            Description = "I display information about a component.";
            Component = component;
            InitWindow();
            //InitButton();
            InitBackground();
            InitTextSurface();
        }

        private void InitWindow()
        {
            Window = new ScreenSurface(Width, Height)
            {
                IsVisible = false,
                IsFocused = false,
                FocusOnMouseClick = true,
            };
            // Window.MouseButtonClicked += MinimizeMaximize;
        }

        private void InitBackground()
        {
            _backgroundSurface = new ScreenSurface(Width - 2, Height - 2);
            _backgroundSurface.Position = (1, 1);
            _backgroundSurface.Surface.Fill(Color.Blue, Color.Tan, '_');
            Window.Children.Add(_backgroundSurface);
        }

        // private void InitButton()
        // {
        //     MaximizeButton = new Button(Window.Title.Length + 2, 3)
        //     {
        //         Theme = new PaperButtonTheme(),
        //         ThemeColors = ThemeColors.Paper,
        //         IsVisible = true,
        //         Text = Window.Title,
        //         TextAlignment = HorizontalAlignment.Center,
        //         Surface = new CellSurface(Window.Title.Length + 2, 3, ButtonCellArray(Window.Title))
        //     };
        //     MaximizeButton.MouseButtonClicked += MaximizeButtonClicked;
        //     //Game.UIManager.Controls.Add(MaximizeButton);
        // }

        // private REXPaintImage.Cell[] ButtonCellArray(string title)
        // {
        //     int width = title.Length + 2; //* 3 
        //     int height = 3;
        //     List<REXPaintImage.Cell> buttonCells = new List<REXPaintImage.Cell>();
        //     for (int i = 0; i < height; i++)
        //     {
        //         for (int j = 0; j < width; j++)
        //         {
        //             //set the button text
        //             int glyph;
        //             if (i == 0)//top row
        //             {
        //                 if (j == width - 1)
        //                     glyph = 191;
        //                 else
        //                     glyph = 196;
        //             }
        //             else
        //             {
        //                 if (j != 0 && j != width - 1)//neither top nor bottom row
        //                     glyph = Component.Name[j - 1];
        //                 else if (j == 0)
        //                     glyph = ' ';
        //                 else
        //                     glyph = 179;
        //             }
        //
        //             REXPaintImage.Cell here = new REXPaintImage.Cell(ThemeColors.Paper.Text, ThemeColors.Paper.ControlBack, glyph);
        //             buttonCells.Add(here);
        //         }
        //     }
        //     return buttonCells.ToArray();
        // }

        private void InitTextSurface()
        {
            _textSurface = new ScreenSurface(Width - 2, Height - 2) 
            {
                UsePixelPositioning = false,
                Position = (1, 1),
            };

            string text = "";
            foreach (string detail in GetDetails())
            {
                text += detail + "\n";
                
            }

            ColoredString details = new ColoredString(text, Color.Blue, Color.Transparent);
            _textSurface.Surface.UsePrintProcessor = true;
            _textSurface.Surface.Print(0,0, details);
            
            Window.Children.Add(_textSurface);
        }

        // private void MaximizeButtonClicked(object sender, MouseEventArgs e)
        // {
        //     if (Window.IsVisible)
        //         Window.Hide();
        //     else
        //         Window.Show();
        //     Game.UiManager.Player.IsFocused = true;
        // }
        public void Print(string[] text)
        {
            Window.Children.Remove(_textSurface);
            InitTextSurface();
        }

        // public override void Update(Console console, TimeSpan delta)
        // {
        //     Print(GetDetails());
        //     if (Window.IsFocused) Game.UiManager.Player.IsFocused = true;
        //     if (MaximizeButton.IsFocused) Game.UiManager.Player.IsFocused = true;
        //     base.Update(console, delta);
        // }

        // public void MinimizeMaximize(object? sender, MouseScreenObjectState mouseScreenObjectState)
        // {
        //     if (args.MouseState.Mouse.RightClicked)
        //     {
        //         if (Window.IsVisible)
        //             Window.Hide();
        //         else
        //             Window.Show();
        //     }
        // }

        public string[] GetDetails() => Component.GetDetails();
    }
}
