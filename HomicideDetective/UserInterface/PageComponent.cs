using System;
using SadConsole;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;

namespace HomicideDetective
{
    //refactor this for clarity upon alpha
    public class PageComponent<T> : RogueLikeComponentBase, IHaveDetails where T : IHaveDetails
    {
        const int Width = 24;
        const int Height = 24;
        public string Name { get; }
        public string Description { get; }
        public ScreenSurface Window { get; private set; }
        public T Component { get; }
        private ScreenSurface _backgroundSurface;
        private ScreenSurface _textSurface;
        
        public PageComponent(T component) : base(true, false, true, true)
        {
            Name = $"PageComponent<{nameof(T)}>";
            Description = "I display information about a component.";
            Component = component;
            InitWindow();
            InitBackground();
            InitTextSurface();
            //InitButton();
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

        private void InitTextSurface()
        {
            _textSurface = new ScreenSurface(Width - 2, Height - 2) 
            {
                UsePixelPositioning = false,
                Position = (1, 1),
            };
            _textSurface.Surface.UsePrintProcessor = true;

            int y = 0;
            foreach (string detail in GetDetails())
            {
                _textSurface.Surface.Print(0, y, detail, Color.Blue);
                y += 1 + (detail.Length / 22);
            }

            Window.Children.Add(_textSurface);
        }

        public override void Update(IScreenObject host, TimeSpan delta)
        {
            Print();
            Window.IsDirty = true;
            base.Update(host, delta);
        }

        // private void MaximizeButtonClicked(object sender, MouseEventArgs e)
        // {
        //     if (Window.IsVisible)
        //         Window.Hide();
        //     else
        //         Window.Show();
        //     Game.UiManager.Player.IsFocused = true;
        // }
        public void Print()
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
