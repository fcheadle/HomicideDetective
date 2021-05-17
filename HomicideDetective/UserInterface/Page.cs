using System.Collections.Generic;
using HomicideDetective.Mysteries;
using SadConsole;
using SadConsole.Components;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace HomicideDetective.UserInterface
{
    /// <summary>
    /// The Message Window and it's background that looks like notepad paper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T> : RogueLikeComponentBase where T : IDetailed
    {
        //SadConsole Controls
        public ScreenSurface TextSurface { get; private set; }
        public ScreenSurface BackgroundSurface { get; private set; }
        public Button PreviousButton { get; private set; }
        public Button NextButton { get; private set; }
        public ControlHost ButtonBar { get; private set; }
        
        //the contents printed by the cursor
        public T Component { get; }
        public List<string> Contents { get; private set; }
        public List<List<string>> PastContents { get; private set; }
        
        public Page(T component) : base(true, false, true, true)
        {
            Component = component;
            InitContents();
            InitWindow();
            //InitButtons();//todo
        }

        private void InitButtons()
        {
            ButtonBar = new ControlHost();
            
            PreviousButton = new Button(4,3)
            {
                MouseArea = new Rectangle((0, 0), (4, 3)),
                CanFocus = false,
            };
            PreviousButton.Surface.DefaultBackground = Color.Tan;
            PreviousButton.Surface.DefaultForeground = Color.Blue;
            PreviousButton.Text = "<-";
            ButtonBar.Add(PreviousButton);

            NextButton = new Button(4,3)
            {
                MouseArea = new Rectangle((BackgroundSurface.Surface.Width - 4, 0), (BackgroundSurface.Surface.Width, 3)),
                CanFocus = false,
            };
            PreviousButton.Surface.DefaultBackground = Color.Tan;
            PreviousButton.Surface.DefaultForeground = Color.Blue;
            PreviousButton.Text = "->";
            
            ButtonBar.Add(PreviousButton);
            
            ButtonBar.Add(NextButton);
            
            BackgroundSurface.SadComponents.Add(ButtonBar);
        }

        private void InitContents()
        {
            PastContents = new List<List<string>>();
            Contents = Component.Details;
        }

        private void InitWindow()
        {
            BackgroundSurface = new ScreenSurface(Program.Width / 3 + 1, Program.Height)
            {
                IsVisible = true,
                IsFocused = false,
                FocusOnMouseClick = true,
            };
            BackgroundSurface.Surface.Fill(Color.Blue, Color.Tan, '_');
            
            TextSurface = new ScreenSurface(Program.Width / 3 - 1, Program.Height)
            {
                IsVisible = true,
                IsFocused = false,
                FocusOnMouseClick = true,
                Position = (1,0)
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
            if(!PastContents.Contains(Contents))
                PastContents.Add(Contents);
            
            var cursor = TextSurface.GetSadComponent<Cursor>();
            cursor.Position = (0, 0);

            Contents = Component.Details;
            foreach (string detail in Contents)
            {
                var answer = new ColoredString($"\r\n{detail}", Color.Blue, Color.Transparent);
                cursor.Print(answer);
            }
        }
    }
}
