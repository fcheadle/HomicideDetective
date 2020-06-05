using GoRogue;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;
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
        public Button MinMaxButton { get; }
        
        internal T Component;

        public PageComponent(BasicEntity parent, Coord position) : base(true, false, true, true)
        {
            Parent = parent;
            Component = (T)(Parent.GetConsoleComponent<T>());
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
                Theme = new PaperTheme(),
                ThemeColors = ThemeColor.Paper
            };

            MinMaxButton = new Button(Name.Length + 2, 3)
            {
                Theme = new PaperButtonTheme(),
                ThemeColors = ThemeColor.Paper
            };

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
                Window.Print(Window.Position.X, Window.Position.Y + i, new ColoredString(text[i].ToString(), Color.DarkBlue, Color.Transparent));
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
