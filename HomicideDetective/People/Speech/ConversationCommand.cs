using System;
using HomicideDetective.UserInterface;
using SadConsole.Input;

namespace HomicideDetective.People.Speech
{
    public class ConversationCommand : KeyCommand
    {
        public string? PassThrough { get; set; }
        public DateTime? RelevantTime { get; set; }
        
        public ConversationCommand(string name, string description, Keys key, bool shift, bool control, bool alt, Action<string> action, string passThrough) 
            : base(name, description, key, shift, control, alt, () => { action(passThrough); })
        {
            PassThrough = passThrough;
        }

        public ConversationCommand(string name, string description, Keys key, bool shift, bool control, bool alt,
            Action<DateTime> action, DateTime relevantDate)
            : base(name, description, key, shift, control, alt, () => { action(relevantDate); })
        {
            RelevantTime = relevantDate;
        }
    }
}