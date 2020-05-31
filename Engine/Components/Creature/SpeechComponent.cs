﻿using Engine.States;
using GoRogue;
using SadConsole;

namespace Engine.Components.Creature
{
    public class SpeechComponent : Component
    {
        //internal SpeechConsole Dialogue;
        internal readonly Font Voice;

        public SpeechComponent(BasicEntity parent) : base(isUpdate: true, isKeyboard: false, isDraw: true, isMouse: false)
        {
            Parent = parent;
        }

        //for now, just print something random to the screen
        //PrintDialogue("Hello there!");
        private void PrintDialogue(string message)
        {
            //Dialogue = new SpeechConsole(Voice, message, new Coord(Parent.Position.X - message.Length / 2, Parent.Position.Y - 1));
            //Dialogue.Print(0, 0, message);
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

        public override void ProcessTimeUnit()
        {
            //todo...
        }
    }
}
