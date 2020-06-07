using Engine.Extensions;
using Engine.Utilities;
using GoRogue;
using GoRogue.MapViews;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Components;
using SadConsole.Controls;
using SadConsole.Renderers;
using System;

namespace Engine.Components
{
    public class KeyboardComponent : KeyboardConsoleComponent //as opposed to my own `component` class, which i should really refactor out
    {
        BasicEntity Parent { get; }
        public Coord Position { get => Parent.Position; }
        public bool IsPaused
        {
            get => SadConsole.Global.CurrentScreen.IsPaused;
            set => SadConsole.Global.CurrentScreen.IsPaused = value;
        }

        public KeyboardComponent(BasicEntity parent)// : base(isUpdate: true, isKeyboard: true, isDraw: false, isMouse: false)
        {
            Parent = parent;
        }


        //public override void Update(SadConsole.Console console, TimeSpan time)
        //{
        //    //ProcessKeyboard(console, Global.KeyboardState, out _);
        //}

        public override void ProcessKeyboard(SadConsole.Console console, SadConsole.Input.Keyboard info, out bool handled)
        {
            if (!IsPaused)
            {
                Direction moveDirection = Direction.NONE;
                //foreach (Keys key in Settings.KeyBindings.Keys)
                //    if (info.IsKeyPressed(key))
                //    {
                //        Settings.TogglePause();
                //    }

                foreach (Keys key in Program.Settings.MovementKeyBindings.Keys)
                {
                    if (info.IsKeyPressed(key))
                    {
                        moveDirection = Program.Settings.MovementKeyBindings[key];
                        break;
                    }
                }

                Coord target = Position + moveDirection;
                if (Parent.CurrentMap.Contains(target))
                    if (Parent.CurrentMap.GetTerrain(target).IsWalkable)
                        Parent.Position += moveDirection;

                handled = false;
                foreach (var action in Program.CurrentGame.Settings.KeyBindings)
                {
                    if (Global.KeyboardState.IsKeyReleased(action.Value))
                    {
                        TakeAction(action.Key);
                    }
                }
            }

            else
            {
                if (Global.KeyboardState.IsKeyReleased(Program.Settings.KeyBindings[GameActions.TogglePause]))
                    TogglePause();
            }
            handled = true;
        }

        //public override string[] GetDetails()
        //{
        //    string[] answer = {
        //        "This is a keyboard component.",
        //        "This entity listens/responds to keyboard input."
        //    };
        //    return answer;
        //}

        //public override void ProcessTimeUnit()
        //{
        //    //don't implement this. at least, no need for now
        //}



        private void TakeAction(GameActions key)
        {
            switch (key)
            {
                case GameActions.RefocusOnPlayer: Parent.IsFocused = true; break;
                case GameActions.DustItemForPrints:
                case GameActions.GetItem:
                case GameActions.LookAtEverythingInSquare:
                case GameActions.LookAtPerson:
                case GameActions.RemoveItemFromInventory:
                case GameActions.TakePhotograph:
                case GameActions.Talk:
                case GameActions.ToggleMenu: ToggleMenu(); break;
                case GameActions.TogglePause: TogglePause(); break;
            }
        }
        private void ToggleMenu()
        {

        }

        private void TogglePause()
        {
            if (IsPaused)
            {
                IsPaused = false;
            }
            else
            {
                IsPaused = true;
            }
        }
    }
}
