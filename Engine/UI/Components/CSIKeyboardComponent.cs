using Engine.Components.UI;
using Engine.Utilities;
using Engine.Utilities.Extensions;
using GoRogue;
using GoRogue.MapViews;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;
using System;

namespace Engine.UI.Components
{
    public class CSIKeyboardComponent : KeyboardConsoleComponent //as opposed to my own `component` class, which i should really refactor out
    {
        BasicEntity Parent { get; }
        public Coord Position { get => Parent.Position; }
        public bool IsPaused
        {
            get => Global.CurrentScreen.IsPaused;
            set => Global.CurrentScreen.IsPaused = value;
        }
        //private Dictionary<GameActions, Keys> KeyBindings => Game.Settings.KeyBindings;
        public CSIKeyboardComponent(BasicEntity parent)// : base(isUpdate: true, isKeyboard: true, isDraw: false, isMouse: false)
        {
            Parent = parent;
        }

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

                foreach (Keys key in Game.Settings.MovementKeyBindings.Keys)
                {
                    if (info.IsKeyPressed(key))
                    {
                        moveDirection = Game.Settings.MovementKeyBindings[key];
                        if (Game.Settings.Mode == GameMode.TurnBased)
                            Game.UIManager.ProcessTimeUnit();
                        break;
                    }
                }

                Coord target = Position + moveDirection;
                if (Parent.CurrentMap.Contains(target))
                    if (Parent.CurrentMap.GetTerrain(target).IsWalkable)
                        Parent.Position += moveDirection;

                handled = false;
                foreach (var action in Game.Settings.KeyBindings)
                {
                    if (Global.KeyboardState.IsKeyReleased(action.Key))
                    {
                        TakeAction(action.Value);
                    }
                }
            }

            else
            {
                foreach (AsciiKey pressed in info.KeysPressed)
                {
                    Keys key = pressed.Key;
                    if (Game.Settings.KeyBindings[key] == GameAction.TogglePause)
                        TogglePause();
                    else if (Game.Settings.KeyBindings[key] == GameAction.ToggleMenu)
                        ToggleMenu();
                }
            }
            handled = true;
        }

        public void TakeAction(GameAction key)
        {
            switch (key)
            {
                //opens a magnifying glass?
                case GameAction.DustItemForPrints:
                case GameAction.GetItem:
                case GameAction.LookAtEverythingInSquare:
                case GameAction.Talk:
                case GameAction.LookAtPerson: OpenCursor(key); break;

                case GameAction.RemoveItemFromInventory:
                case GameAction.TakePhotograph:

                case GameAction.ToggleMenu: ToggleMenu(); break;
                case GameAction.TogglePause: TogglePause(); break;
                case GameAction.RefocusOnPlayer: Parent.IsFocused = true; break;
            }
        }

        private void OpenCursor(GameAction purpose)
        {
            Parent.Components.Add(new MagnifyingGlassComponent(Parent, Parent.Position, purpose));
        }

        public void ToggleMenu()
        {
            Game.Menu.Toggle();
        }

        public void TogglePause()
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
