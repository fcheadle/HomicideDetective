using Engine.UI;
using GoRogue;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components
{
    class SpeechComponent : ComponentBase
    {
        internal SpeechConsole Dialogue;
        internal readonly Font Voice;

        //for now, just print something random to the screen
        //PrintDialogue("Hello there!");
        private void PrintDialogue(string message)
        {
            Dialogue = new SpeechConsole(Voice, message, new Coord(Parent.Position.X - message.Length / 2, Parent.Position.Y - 1));
            Dialogue.Print(0, 0, message);
        }
    }
}
