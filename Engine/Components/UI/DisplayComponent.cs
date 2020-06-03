using Engine.Maps.Areas;
using Engine.UI;
using GoRogue;
using SadConsole;
using SadConsole.Input;
using System;
using Color = Microsoft.Xna.Framework.Color;
using Console = SadConsole.Console;

namespace Engine.Components.UI
{
    public class DisplayComponent<T> : Component, IDisplay where T : Component
    {
        public ScrollingConsole Console { get => Page.Console; }
        public Window Window { get => Page; }
        public Page Page;
        internal T Component; 

        public DisplayComponent(BasicEntity parent, Coord position) : base(true, false, true, true)
        {
            Parent = parent;
            Component = (T)(Parent.GetConsoleComponent<T>());
            Name = "Display for " + Component.Name;
            Page = new Page(Component.Name, GetDetail());
            //Display.Position = new Coord(0, 0);
            Page.IsVisible = true;
            Page.IsFocused = true;
            Page.FocusOnMouseClick = true;
            Page.Position = position;
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
            Page.Fill(Color.Blue, Color.Tan, '_');
            for (int i = 0; i < text.Length; i++)
            {
                Page.Print(0, i, new SadConsole.ColoredString(text[i].ToString(), Color.DarkBlue, Color.Transparent));
                //Display.Add(text[i]);
            }
        }
        public void Print(Area[] areas)
        {
            Page.Fill(Color.Blue, Color.Tan, '_');
            //Display.Fill(Color.Transparent, Display.DefaultBackground, 0);
            for (int i = 0; i < areas.Length; i++)
            {
                Page.Print(0, i, new SadConsole.ColoredString(areas[i].Name, Color.DarkBlue, Color.Transparent));
                //Display.Add(areas[i].Name);
            }
        }
        public override void Draw(Console console, TimeSpan delta)
        {
            Page.Draw(delta);
            base.Draw(console, delta);
        }

        public override void Update(Console console, TimeSpan delta)
        {
            Page.Update(delta);
            base.Update(console, delta);
        }

        public override void ProcessMouse(Console console, MouseConsoleState state, out bool handled)
        {
            Page.ProcessMouse(state);
            base.ProcessMouse(console, state, out handled);
        }
        public override string[] GetDetails()
        {
            return Component.GetDetails();
        }

        internal void Print()
        {

        }

        public override void ProcessTimeUnit()
        {
            Print(Component.GetDetails());
        }
    }
}
