using Engine.Maps.Areas;
using GoRogue;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System;
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
        public int PageNumber;
        public Button BackPageButton;
        public Button NextPageButton;
        private const string _title = "////////////////////////////////";
        public NotePadComponent(BasicEntity parent, Coord position) : base(true, false, true, true)
        {
            
            Parent = parent;
            Name = "Notepad";
            Window = new Window(_width, _height)
            {
                DefaultBackground = Color.Tan,
                Title = Name,
                TitleAlignment = HorizontalAlignment.Center,
                IsVisible = true,
                IsFocused = false,
                FocusOnMouseClick = false,
                Position = position,
                ViewPort = new GoRogue.Rectangle(0, 0, _width, _height),
                Theme = new PaperTheme(),
                ThemeColors = PaperTheme.Colors,
                CanTabToNextConsole = true,
            };
            BackPageButton = new Button(1) { Position = new Coord(0, _height - 1), Text = "<" };
            BackPageButton.MouseButtonClicked += BackButton_Clicked;
            Window.Add(BackPageButton);

            NextPageButton = new Button(1) { Position = new Coord(_width - 1, _height - 1), Text = ">" };
            NextPageButton.MouseButtonClicked += NextButton_Clicked;
            Window.Add(NextPageButton); 
            Window.Show();
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
            Window.Fill(Color.Blue, Color.Tan, '_');
            //Display.Fill(Color.Transparent, Display.DefaultBackground, 0);
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
            //Window.Title = CurrentPage.Title;
            Window.Update(delta);
            base.Update(console, delta);
        }

        public override void ProcessMouse(Console console, MouseConsoleState state, out bool handled)
        {
            //Console.ProcessMouse(state);
            Window.ProcessMouse(state);
            base.ProcessMouse(console, state, out handled);
        }
        public override string[] GetDetails()
        {
            string[] answer = { _title };
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
            //do nothing
        }
    }
}
