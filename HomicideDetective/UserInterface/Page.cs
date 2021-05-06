using System;
using HomicideDetective.Mysteries;
using SadConsole;
using SadConsole.Components;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using SadRogue.Integration.Components;

namespace HomicideDetective
{
    //refactor this for clarity upon alpha
    public class PageComponent<T> : RogueLikeComponentBase where T : IDetailed
    {
        // const int Width = 24;
        // const int Height = 24;
        public ScreenSurface TextSurface { get; private set; }
        public ScreenSurface BackgroundSurface { get; private set; }
        public T Component { get; }
        
        public PageComponent(T component) : base(true, false, true, true)
        {
            Component = component;
            InitWindow();
        }

        private void InitWindow()
        {
            
            BackgroundSurface = new ScreenSurface(Program.Width / 3, Program.Height)
            {
                IsVisible = true,
                IsFocused = false,
                FocusOnMouseClick = true,
                Position = (1,1)
            };
            BackgroundSurface.Surface.Fill(Color.Blue, Color.Tan, '_');
            
            TextSurface = new ScreenSurface(Program.Width / 3 - 2, Program.Height - 2)
            {
                IsVisible = true,
                IsFocused = false,
                FocusOnMouseClick = true,
                Position = (1,1)
            };
            
            var cursor = new Cursor()
            {
                IsVisible = false,
                UsePrintEffect = true,
            };
            TextSurface.SadComponents.Add(cursor);
            BackgroundSurface.Children.Add(TextSurface);
        }
        public void Print()
        {
            TextSurface.Surface.Clear();

            var cursor = TextSurface.GetSadComponent<Cursor>();
            cursor.Position = (0, 0);
            foreach (string detail in Component.Details)
            {
                var answer = new ColoredString($"\r\n{detail}", Color.Blue, Color.Transparent);
                cursor.Print(answer);
            }
        }
    }
}
