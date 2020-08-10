using Engine.Utilities;
using Engine.Utilities.Extensions;
using GoRogue;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;
using System;

namespace Engine.UI.Components
{
    public class CsiKeyboardComponent : KeyboardConsoleComponent //as opposed to my own `component` class, which i should really refactor out
    {
        BasicEntity Parent { get; }
        public Coord Position { get => Parent.Position; }
        public bool IsPaused
        {
            get => Global.CurrentScreen.IsPaused;
            set => Global.CurrentScreen.IsPaused = value;
        }

        public CsiKeyboardComponent(BasicEntity parent)
        {
            Parent = parent;
        }

        public override void ProcessKeyboard(SadConsole.Console console, SadConsole.Input.Keyboard info, out bool handled)
        {
            if (!IsPaused)
            {
                Direction moveDirection = Direction.NONE;
                foreach (Keys key in Game.Settings.MovementKeyBindings.Keys)
                {
                    if (info.IsKeyPressed(key))
                    {
                        moveDirection = Game.Settings.MovementKeyBindings[key];
                        if (Game.Settings.Mode == GameMode.TurnBased)
                            Game.UiManager.ProcessTimeUnit();
                        break;
                    }
                }

                Coord target = Position + moveDirection;
                if (Parent.CurrentMap.Contains(target))
                    if (Parent.CurrentMap.GetTerrain(target).IsWalkable)
                        Parent.Position += moveDirection;

                handled = true;
                foreach (var action in Game.Settings.KeyBindings)
                {
                    if (Global.KeyboardState.IsKeyPressed(action.Key))
                    {
                        TakeAction(action.Value);
                    }
                }
            }

            else
            {
                foreach (AsciiKey pressed in info.KeysPressed)
                {
                    if (Game.Settings.KeyBindings.ContainsKey(pressed.Key))
                    {
                        Keys key = pressed.Key;
                        if (Game.Settings.KeyBindings[key] == GameAction.TogglePause)
                            TogglePause();
                        else if (Game.Settings.KeyBindings[key] == GameAction.ToggleMenu)
                            ToggleMenu();
                    }
                }
            }
            handled = true;
        }

        public void TakeAction(GameAction key)
        {
            switch (key)
            {
                //opens a magnifying glass?
                case GameAction.DustItemForPrints: OpenCursor(GameAction.DustItemForPrints); break;
                case GameAction.GetItem: OpenCursor(GameAction.GetItem); break;
                case GameAction.LookAtEverythingInSquare: OpenCursor(GameAction.LookAtEverythingInSquare); break;
                case GameAction.Talk: OpenCursor(GameAction.Talk); break;
                case GameAction.LookAtPerson: OpenCursor(key); break;

                case GameAction.RemoveItemFromInventory: DropItem(); break;
                case GameAction.TakePhotograph: TakePhotograph(); break;

                case GameAction.ToggleMenu: ToggleMenu(); break;
                case GameAction.TogglePause: TogglePause(); break;
                case GameAction.RefocusOnPlayer: Parent.IsFocused = true; break;
            }
        }

        private void TakePhotograph()
        {
            throw new NotImplementedException();
        }

        private void DropItem()
        {
            throw new NotImplementedException();
        }

        private void OpenCursor(GameAction purpose)
        {
            Parent.Components.Add(new MagnifyingGlassComponent(Parent, Parent.Position, purpose));
        }

        public void ToggleMenu()
        {
            Game.SwitchUserInterface();
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
