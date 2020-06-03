using Engine.Maps.Areas;
using Engine.UI;
using GoRogue;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;
using Console = SadConsole.Console;

namespace Engine.Components.UI
{
    public class NotebookComponent : Component, IDisplay
    {
        public Page CurrentPage { get => Pages[PageNumber]; }
        public ScrollingConsole Console { get => CurrentPage.Console; }
        public Window Window { get => CurrentPage; }
        public int PageNumber;
        public List<Page> Pages = new List<Page>();
        public Button BackPageButton;
        public Button NextPageButton;
        private const string _title = "////////////////////////////////";
        public NotebookComponent(BasicEntity parent, Coord position) : base(true, false, true, true)
        {
            Parent = parent;
            Pages.Add(new Page(_title, GetDetail()));
            //Display = new ScrollingConsole(32, 32);// new Page(_title, GetDetail());
            //Display.Position = new Coord(0, 0);
            Console.IsVisible = true;
            Console.IsFocused = true;
            Console.FocusOnMouseClick = true;
            Console.Position = position;
            BackPageButton = new Button(1) { Position = new Coord(0, CurrentPage.Height - 1), Text = "<" };
            BackPageButton.MouseButtonClicked += BackButton_Clicked;
            CurrentPage.Add(BackPageButton);

            NextPageButton = new Button(1) { Position = new Coord(CurrentPage.Width - 1, CurrentPage.Height - 1), Text = ">" };
            NextPageButton.MouseButtonClicked += NextButton_Clicked;
            CurrentPage.Add(NextPageButton);
            Window.Show();
        }

        private string GetDetail()
        {
            string answer = "";

            foreach (string s in GetDetails())
                answer += s + ". ";

            return answer;
        }

        public void Print(string[] text)
        {
            Console.Fill(Color.Blue, Color.Tan, '_');
            for (int i = 0; i < text.Length; i++)
            {
                Console.Print(0, i, new SadConsole.ColoredString(text[i].ToString(), Color.DarkBlue, Color.Transparent));
                //Display.Add(text[i]);
            }
        }
        public void Print(Area[] areas)
        {
            Console.Fill(Color.Blue, Color.Tan, '_');
            //Display.Fill(Color.Transparent, Display.DefaultBackground, 0);
            for (int i = 0; i < areas.Length; i++)
            {
                Console.Print(0, i, new SadConsole.ColoredString(areas[i].Name, Color.DarkBlue, Color.Transparent));
                //Display.Add(areas[i].Name);
            }
        }
        public override void Draw(Console console, TimeSpan delta)
        {
            Window.Draw(delta);
            base.Draw(console, delta);
        }

        public override void Update(Console console, TimeSpan delta)
        {
            Window.Title = CurrentPage.Title;
            Window.Update(delta);
            base.Update(console, delta);
        }

        public override void ProcessMouse(Console console, MouseConsoleState state, out bool handled)
        {
            Console.ProcessMouse(state);
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
