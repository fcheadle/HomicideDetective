using System;
using SadConsole.Input;

namespace HomicideDetective.UserInterface
{
    public class KeyCommand
    {
        public string Name { get; }
        public string Description { get; }
        public Keys Key { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }
        public bool Alt { get; set; }
        public Action Action { get; }
        
        public KeyCommand(string name, string description, Keys key, bool shift, bool control, bool alt, Action action)
        {
            Name = name;
            Description = description;
            Key = key;
            Shift = shift;
            Control = control;
            Alt = alt;
            Action = action;
        }
    }
}