using Microsoft.Xna.Framework;
using Console = SadConsole.Console;
namespace Engine.UI
{
    public enum MessageTypes
    {
        Warning,
        Status,
        Problem,
        Battle
    }
    internal class MessageConsole : Console
    {
        internal MessageConsole(int width, int height) : base(width, height)
        {
            IsCursorDisabled = false;
            Cursor.IsVisible = false;
            UseMouse = true;
            
            UseKeyboard = true;
            DefaultBackground =  Color.Tan;
            Fill(Color.Blue, Color.Tan, '_');
            this[0].CopyAppearanceTo(Cursor.PrintAppearance);
        }

        internal void Print(string[] text, MessageTypes type = MessageTypes.Warning)
        {
            Fill(Color.Blue, Color.Tan, '_');
            for (int i = 0; i < text.Length; i++)
            {
                base.Print(0, i, new SadConsole.ColoredString(text[i], Color.DarkBlue, Color.Transparent));
            }
        }
        internal void Print(Maps.Area[] areas, MessageTypes type = MessageTypes.Warning)
        {
            Fill(Color.Blue, Color.Tan, '_');
            for (int i = 0; i < areas.Length; i++)
            {
                base.Print(0, i, new SadConsole.ColoredString(areas[i].Name, Color.DarkBlue, Color.Transparent));
            }
        }
    }
}