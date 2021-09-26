using System.Collections.Generic;
using System.Linq;
using HomicideDetective.People;
using SadConsole.Input;

namespace HomicideDetective.UserInterface
{
    public class CommandContext
    {
        public KeyCommand[] Commands { get; }
        public ISubstantive? Subject { get; }
        
        public CommandContext(IEnumerable<KeyCommand> contextCommands, ISubstantive? subject = null)
        {
            Subject = subject;
            Commands = contextCommands.ToArray();
        }

        public bool ProcessKeyboard(Keyboard keyboard)
        {
            //actions
            var shft = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);
            var alt = keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt);
            var ctrl = keyboard.IsKeyDown(Keys.LeftControl) || keyboard.IsKeyDown(Keys.RightControl);
            
            foreach (var key in keyboard.KeysDown)
            {
                var command = Commands.FirstOrDefault(cmd => cmd.Key == key && cmd.Shift == shft && cmd.Alt == alt && cmd.Control == ctrl);
                if (command != null)
                {
                    command.Action.Invoke();
                    return true;
                }
            }

            return false;
        }

        public static CommandContext CrimeSceneInvestigationContext()
        {
            var commands = new[]
            {
                new KeyCommand("Talk", "", Keys.T, false, false, false, UserInterface.Commands.Talk),
                new KeyCommand("Look Around", "", Keys.L, false, false, false, UserInterface.Commands.LookAround),
                new KeyCommand("Inspect", "", Keys.I, false, false, false, UserInterface.Commands.Inspect),
                new KeyCommand("Help", "", Keys.OemQuestion, false, false, false, UserInterface.Commands.PrintCommands),
                new KeyCommand("Fast Travel", "", Keys.M, false, false, false, UserInterface.Commands.NextMap),
                new KeyCommand("Back Page", "", Keys.Home, false, false, false, UserInterface.Commands.BackPage),
                new KeyCommand("Forward Page", "", Keys.End, false, false, false, UserInterface.Commands.ForwardPage),
                new KeyCommand("Scroll Up", "", Keys.PageUp, false, false, false, UserInterface.Commands.UpOneLine),
                new KeyCommand("Scroll Down", "", Keys.PageDown, false, false, false, UserInterface.Commands.DownOneLine),
            };
            return new CommandContext(commands);
        }
    }
}