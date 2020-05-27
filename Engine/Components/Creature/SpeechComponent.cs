using Engine.UI;
using GoRogue;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components.Creature
{
    public class SpeechComponent : ComponentBase
    {
        internal SpeechConsole Dialogue;
        internal readonly Font Voice;

        public SpeechComponent() : base(isUpdate: true, isKeyboard: false, isDraw: true, isMouse: false)
        {
        }

        //for now, just print something random to the screen
        //PrintDialogue("Hello there!");
        private void PrintDialogue(string message)
        {
            Dialogue = new SpeechConsole(Voice, message, new Coord(Parent.Position.X - message.Length / 2, Parent.Position.Y - 1));
            Dialogue.Print(0, 0, message);
        }

        public override void ProcessGameFrame()
        {

        }

        public override string[] GetDetails()
        {
            string[] answer =
            {
                "this is a speech component.",
                "The entity with this component can speak."
            };
            return answer;//
        }
    }
}
