using GoRogue;
using Microsoft.Xna.Framework.Graphics;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System;
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
        
        internal T Component;
        DrawingSurface _surface;
        bool _hasDrawn;
        public PageComponent(BasicEntity parent, Coord position) : base(true, false, true, true)
        {
            Parent = parent;
            Component = (T)Parent.GetConsoleComponent<T>();
            Name = "Display for " + Component.Name;
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
                Theme = new PaperWindowTheme(),
                ThemeColors = ThemeColor.Paper
            };
            Window.ThemeColors.RebuildAppearances();

            MaximizeButton = new Button(Name.Length + 2, 3)
            {
                Theme = new PaperButtonTheme(),
                ThemeColors = ThemeColor.Paper
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

        public void AddContent(string content)
        {
            _content.Append(content);
        }
        public void Print(string[] text)
        {
            Window.Fill(Color.Blue, Color.Tan, '_');
            for (int i = 0; i < text.Length; i++)
            {
                Window.Print(1, 1 + i, text[i]);
            }
        }
        public override void Draw(Console console, TimeSpan delta)
        {
            base.Draw(console, delta);
        }

        public override void Update(Console console, TimeSpan delta)
        {
            //There's probably a better way to do this
            Window.Remove(_surface);
            
            _surface.Surface.Fill(Color.Blue, Color.Tan, '_');
            int i = 0;
            foreach (string text in Component.GetDetails())
            {
                _surface.Surface.Print(0, i, text);
                i++;
            }
            Window.Add(_surface);
            base.Update(console, delta);
        }

        public override void ProcessMouse(Console console, MouseConsoleState state, out bool handled)
        {
            if (state.IsOnConsole && state.Mouse.RightClicked)
            {
                Window.Hide();
                MaximizeButton.IsVisible = true;
            }
            handled = true;
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
