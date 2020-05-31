using Engine.Extensions;
using GoRogue;
using GoRogue.MapViews;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using System;

namespace Engine.Components
{
    public class KeyboardComponent : Component
    {
        public Coord Position { get => Parent.Position; }
        public KeyboardComponent(BasicEntity parent) : base(isUpdate: true, isKeyboard: true, isDraw: false, isMouse: false)
        {
            Parent = parent;
        }


        public override void Update(SadConsole.Console console, TimeSpan time)
        {
            ProcessKeyboard(console, Global.KeyboardState, out _);
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

            Coord target = Position + moveDirection;
            if (Parent.CurrentMap.Contains(target))
                if (Parent.CurrentMap.GetTerrain(target).IsWalkable)
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

        public override void ProcessTimeUnit()
        {
            //don't implement this. at least, no need for now
        }
    }
}
