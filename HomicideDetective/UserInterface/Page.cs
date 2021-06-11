using System.Collections.Generic;
using System.Linq;
using HomicideDetective.Happenings;
using HomicideDetective.People;
using SadConsole;
using SadConsole.Components;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace HomicideDetective.UserInterface
{
    /// <summary>
    /// The Message Window and it's background that looks like notepad paper
    /// </summary>
    public class Page : RogueLikeComponentBase
    {
        //SadConsole Controls
        public ScreenSurface TextSurface { get; private set; }
        public ScreenSurface BackgroundSurface { get; private set; }
        public int Width { get; }
        public int Height { get; }
        
        //the contents printed by the cursor
        public List<string> Contents { get; private set; }= new List<string>();
        public int PageNumber { get; private set; } = 0;
        
        public Page(int width, int height) : base(true, false, true, true)
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
            
            TextSurface = new ScreenSurface(Width - 3, Height * 3)
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
            TextSurface.SadComponents.Add(cursor);
            BackgroundSurface.Children.Add(TextSurface);
        }
        
        private void PrintContents(int index)
        {
            var cursor = TextSurface.GetSadComponent<Cursor>();
            Clear();
            cursor.Position = (0, 0);
            cursor.Print(new ColoredString($"{Contents[index]}", Color.Blue, Color.Transparent));
        }

        private void PrintContents() => PrintContents(PageNumber);
        
        public void Write(string contents)
        {
            Clear();
            if(!Contents.Any() || !Contents[PageNumber].Contains(contents))
                Contents.Add(contents);

            PageNumber = Contents.Count - 1;
            PrintContents();
        }

        public void Write(IEnumerable<string> contents)
        {
            Clear();
            var content = "";
            foreach (var thing in contents)
                content += $"{thing}\r\n";
            
            Write(content);
        }

        public void Write(Memory happening)
        {
            List<string> contents = new();
            contents.Add(happening.When.ToString());
            contents.Add($"Location: ${happening.Where}");

            //var things = "Things involved: ";
            //foreach(var thing in happening.What)
            //    things += $"{thing}, "; 
            var people = "People Involved: ";
            foreach (var person in happening.Who)
                people += $"{person}, ";
            contents.Add($"Occurence: ${happening.What}");
        }

        public void Write(Substantive substantive)
        {
            List<string> contents = new();
            contents.Add(substantive.Name!);
            // contents.Add(substantive.Description!);
            contents.Add(substantive.GenerateDetailedDescription());
            contents.AddRange(substantive.Details);
            Write(contents);
        }

        public static void BackPage()
        {
            var page = Program.CurrentGame.MessageWindow;
            if (page.PageNumber > 0)
                page.PrintContents(--page.PageNumber);
        }
        
        public static void ForwardPage()
        {
            var page = Program.CurrentGame.MessageWindow;
            if (page.PageNumber < page.Contents.Count - 1)
                page.PrintContents(++page.PageNumber);
        }

        public static void UpOneLine()
        {
            var page = Program.CurrentGame.MessageWindow;
            page.TextSurface.Surface.ViewPosition -= (0, 1);
            page.TextSurface.IsDirty = true;
        }

        public static void DownOneLine()
        {
            var page = Program.CurrentGame.MessageWindow;
            page.TextSurface.Surface.ViewPosition += (0, 1);
            page.TextSurface.IsDirty = true;
        }

        //temporary - for testing
        public static void WriteGarbage()
        {
            var page = Program.CurrentGame.MessageWindow;
            page.Clear();
            string contents = "home ";
            for (int i = 0; i < 1000; i++)
                contents += $"{(char)(i % 256)} ";
            
            page.Write(contents);
        }

        public void Clear()
        {
            TextSurface.Surface.Clear();
        }
    }
}
