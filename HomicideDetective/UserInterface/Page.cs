using System;
using HomicideDetective.Mysteries;
using SadConsole;
using SadConsole.Components;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;

namespace HomicideDetective
{
    //refactor this for clarity upon alpha
    public class PageComponent<T> : RogueLikeComponentBase, IDetailed where T : IDetailed
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
        }

        private void InitWindow()
        {
            Window = new ScreenSurface(Width, Height)
            {
                IsVisible = false,
                IsFocused = false,
                FocusOnMouseClick = true,
            };
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
            var cursor = new Cursor {IsVisible = false};
            _textSurface.SadComponents.Add(cursor);
            foreach (string detail in GetDetails())
            {
                var answer = new ColoredString($"\r\n{detail}", Color.Blue, Color.Transparent);
                cursor.Print(answer);
            }

            Window.Children.Add(_textSurface);
        }

        public override void Update(IScreenObject host, TimeSpan delta)
        {
            Print();
            Window.IsDirty = true;
            base.Update(host, delta);
        }
        public void Print()
        {
            Window.Children.Remove(_textSurface);
            InitTextSurface();
        }

        public string[] GetDetails() => Component.GetDetails();
        public string[] AllDetails() => Component.AllDetails();
    }
}
