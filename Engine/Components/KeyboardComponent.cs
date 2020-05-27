using Engine.Extensions;
using Engine.Maps;
using GoRogue;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components
{
    public class KeyboardComponent : ComponentBase
    {
        public Coord Position { get => Parent.Position;}
        public KeyboardComponent() : base(isUpdate: true, isKeyboard: true, isDraw: false, isMouse: false)
        {
        }


        public override void ProcessGameFrame()
        {
            ProcessKeyboard((SadConsole.Console)Parent, Global.KeyboardState, out _);
        }

        public override void ProcessKeyboard(SadConsole.Console console, SadConsole.Input.Keyboard info, out bool handled)
        {
            Direction moveDirection = Direction.NONE;
            //foreach (Keys key in Settings.KeyBindings.Keys)
            //    if (info.IsKeyPressed(key))
            //    {
            //        Settings.TogglePause();
            //    }

            foreach (Keys key in Settings.MovementKeyBindings.Keys)
            {
                if (info.IsKeyPressed(key))
                {
                    moveDirection = Settings.MovementKeyBindings[key];
                    break;
                }
            }
            if(Program.CurrentState.Map != null)
                if(Program.CurrentState.Map.Contains(Position + moveDirection))
                    if(Program.CurrentState.Map.GetTerrain(Position + moveDirection) != null)
                        if(Program.CurrentState.Map.GetTerrain(Position + moveDirection).IsWalkable)
                            Parent.Position += moveDirection;

            if (moveDirection != Direction.NONE)
                handled = true;
            else
                handled = ((BasicEntity)Parent).MoveIn(moveDirection);
        }

        public override string[] GetDetails()
        {
            string[] answer = {
                "This is a keyboard component.",
                "This entity listens/responds to keyboard input."
            };
            return answer;
        }
    }
}
