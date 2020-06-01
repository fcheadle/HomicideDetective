using Engine.Maps.Areas;
using GoRogue;
using SadConsole;
using Color = Microsoft.Xna.Framework.Color;
using Console = SadConsole.Console;

namespace Engine.Components.UI
{
    public class DisplayComponent<T> : Component where T : Component
    {
        //todo - left off here, going to create a new class for a draggablle notepad
        public Window Window;
        public Console Display;
        internal T Component; 

        public DisplayComponent(BasicEntity parent, Coord position) : base(true, false, true, true)
        {
            Parent = parent; 
            Component = (T)(Parent.GetConsoleComponent<T>());
            Display = new Console(24, 24)
            {
                DefaultBackground = Color.Tan,
                Position = position,
                IsVisible = true,
                IsExclusiveMouse = true,
                
            };
        }

        public void Print(string[] text)
        {

            Display.Fill(Color.Blue, Color.Tan, '_');
            //Display.Fill(Color.Transparent, Display.DefaultBackground, 0);
            for (int i = 0; i < text.Length; i++)
            {
                Display.Print(1, i, new SadConsole.ColoredString(text[i].ToString(), Color.DarkBlue, Color.Transparent));
            }
        }
        public void Print(Area[] areas)
        {
            Display.Fill(Color.Blue, Color.Tan, '_');
            //Display.Fill(Color.Transparent, Display.DefaultBackground, 0);
            for (int i = 0; i < areas.Length; i++)
            {
                Display.Print(0, i, new SadConsole.ColoredString(areas[i].Name, Color.DarkBlue, Color.Transparent));
            }
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
