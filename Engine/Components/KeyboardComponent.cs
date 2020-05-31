using Engine.Extensions;
using GoRogue;
using GoRogue.MapViews;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using System;

namespace Engine.Components
{
    public class KeyboardComponent : ComponentBase
    {
        public Coord Position { get => Parent.Position; }
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
            if (Parent.CurrentMap.Contains(Position + moveDirection))
                if (Parent.CurrentMap.GetTerrain(Position + moveDirection) != null)
                    if (Parent.CurrentMap.GetTerrain(Position + moveDirection).IsWalkable)
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
