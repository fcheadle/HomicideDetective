using GoRogue;
using System;
using SadConsole;
using Console = SadConsole.Console;
using Color = Microsoft.Xna.Framework.Color;

namespace Engine.UI
{
    internal class SpeechConsole : Console
    {
        private ColoredString _message;
        private Color fore;
        private Color back;
        public SpeechConsole(Font voice, string statement, Coord position) : base(statement.Length, 1, voice)//for now
        {
            Position = position;
            fore = Color.White;
            back = Color.Black;
            IsFocused = true;
            _message = new ColoredString(statement, fore, back);
            Print(0,0,_message);
        }

        public override void Update(TimeSpan timeElapsed)
        {
            fore = Utils.Colors.FadeOut(fore);
            back = Utils.Colors.FadeOut(back);
            _message.SetForeground(fore);
            _message.SetBackground(back);
            Clear();
            Print(0, 0, _message);
            base.Update(timeElapsed);
        }
    }
}
