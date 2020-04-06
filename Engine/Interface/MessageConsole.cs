using Microsoft.Xna.Framework;
using Console = SadConsole.Console;
namespace Engine.Interface
{
    internal class MessageConsole : Console
    {
        private Color _semiTransparentBlack;

        public enum MessageTypes
        {
            Warning,
            Status,
            Problem,
            Battle
        }

        public MessageConsole(int width, int height) : base(width, height)
        {
            IsCursorDisabled = false;
            Cursor.IsVisible = false;
            UseMouse = true;
            
            UseKeyboard = false;
            _semiTransparentBlack.A = 128;
            DefaultBackground =  _semiTransparentBlack;
            Fill(Color.Blue, Color.Tan, '_');
            this[0].CopyAppearanceTo(Cursor.PrintAppearance);
        }

        public void Print(string text, MessageTypes type = MessageTypes.Warning)
        {
            Color color;

            switch (type)
            {
                case MessageTypes.Warning:
                    color = Color.PaleVioletRed;
                    break;
                case MessageTypes.Problem:
                    color = Color.Orange;
                    break;
                case MessageTypes.Battle:
                    color = Color.LawnGreen;
                    break;
                case MessageTypes.Status:
                default:
                    color = Color.LightGray;
                    break;
            }
            Clear();
            //Fill(Color.Blue, Color.Tan, '_');
            Cursor.NewLine().Print(new SadConsole.ColoredString("* " + text, color, Color.Transparent) { IgnoreBackground = true });
        }
    }
}