using System.Collections.Generic;
using System.Linq;
using SadConsole;
using SadConsole.Components;
using SadRogue.Integration.Components;
using SadRogue.Primitives;
#pragma warning disable 8618

namespace HomicideDetective.UserInterface
{
    /// <summary>
    /// The Message Window and it's background that looks like notepad paper
    /// </summary>
    public class PageWindow : RogueLikeComponentBase
    {
        //SadConsole Controls
        public ScreenSurface TextSurface { get; private set; }
        public ScreenSurface BackgroundSurface { get; private set; }
        public int Width { get; }
        public int Height { get; }
        
        //the contents printed by the cursor
        // public List<PageContentSource> Contents { get; private set; }= new ();
        public List<string> Contents { get; set; } = new();
        public int PageNumber { get; set; }
        
        public PageWindow(int width, int height) : base(true, false, true, true)
        {
            Width = width;
            Height = height;
            InitWindow();
        }

        private void InitWindow()
        {
            BackgroundSurface = new ScreenSurface(Width, Height)
            {
                IsVisible = true,
                IsFocused = false,
                FocusOnMouseClick = true,
            };
            BackgroundSurface.Surface.Fill(Color.Blue, Color.Tan, '_');
            
            TextSurface = new ScreenSurface(Width - 3, Height * 2)
            {
                IsVisible = true,
                IsFocused = false,
                FocusOnMouseClick = true,
                Position = (1,1)
            };
            TextSurface.Surface.View = new Rectangle((0, 0), (Width - 2, Height - 2));
            TextSurface.Surface.DefaultBackground = Color.Transparent;
            TextSurface.Surface.DefaultForeground = Color.Blue;
            var cursor = new Cursor()
            {
                IsVisible = false,
                UsePrintEffect = true,
            };
            cursor.AutomaticallyShiftRowsUp = true;
            TextSurface.SadComponents.Add(cursor);
            BackgroundSurface.Children.Add(TextSurface);
        }

        public void PrintContents(int index)
        {
            var cursor = TextSurface.GetSadComponent<Cursor>();
            Clear();
            cursor.Position = (0, 0);
            if (Program.CurrentGame != null)
            {
                cursor.Print(new ColoredString(Program.CurrentGame.CurrentTime.ToString(), Color.Blue, Color.Transparent));
                cursor.NewLine();
            }
            cursor.Print(new ColoredString(Contents[index], Color.Blue, Color.Transparent));
        }

        public void Write(string contents)
        {
            AddNewContents(contents);
            PrintContents(PageNumber);
        }

        private void AddNewContents(string contents)
        {
            Contents.Add(contents);
            PageNumber = Contents.Count - 1;
        }

        public void Clear()
        {
            TextSurface.Surface.Clear();
        }
    }
}
