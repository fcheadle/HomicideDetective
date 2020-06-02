using Engine.Maps.Areas;
using GoRogue;
using SadConsole;
using SadConsole.Input;
using System;
using Color = Microsoft.Xna.Framework.Color;
using Console = SadConsole.Console;

namespace Engine.Components.UI
{
    public class DisplayComponent<T> : Component where T : Component
    {
        //todo - left off here, going to create a new class for a draggable notepad
        public Notepad Display;
        internal T Component; 

        public DisplayComponent(BasicEntity parent, Coord position) : base(true, false, true, true)
        {
            Parent = parent;
            Component = (T)(Parent.GetConsoleComponent<T>());
            Name = "Display for " + Component.Name;
            Display = new Notepad(Name);
            //Display.Position = new Coord(0, 0);
            Display.IsVisible = true;
            Display.IsFocused = true;
            Display.FocusOnMouseClick = true;
            //OnClick
        }

        public void Print(string[] text)
        {
            //Display.Fill(Color.Blue, Color.Tan, '_');
            //Display.Fill(Color.Transparent, Display.DefaultBackground, 0);
            for (int i = 0; i < text.Length; i++)
            {
                //Display.Print(1, i, new SadConsole.ColoredString(text[i].ToString(), Color.DarkBlue, Color.Transparent));
                Display.Add(text[i]);
            }
        }
        public void Print(Area[] areas)
        {
            Display.Fill(Color.Blue, Color.Tan, '_');
            //Display.Fill(Color.Transparent, Display.DefaultBackground, 0);
            for (int i = 0; i < areas.Length; i++)
            {
                //Display.Print(0, i, new SadConsole.ColoredString(areas[i].Name, Color.DarkBlue, Color.Transparent));
                Display.Add(areas[i].Name);
            }
        }
        public override void Draw(Console console, TimeSpan delta)
        {
            Display.Draw(delta);
            base.Draw(console, delta);
        }

        public override void Update(Console console, TimeSpan delta)
        {
            Display.Update(delta);
            base.Update(console, delta);
        }

        public override void ProcessMouse(Console console, MouseConsoleState state, out bool handled)
        {
            Display.ProcessMouse(state);
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
