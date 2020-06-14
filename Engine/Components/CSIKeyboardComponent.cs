using Engine.Components.UI;
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
using System.Collections.Generic;

namespace Engine.Components
{
    public class CSIKeyboardComponent : KeyboardConsoleComponent //as opposed to my own `component` class, which i should really refactor out
    {
        BasicEntity Parent { get; }
        public Coord Position { get => Parent.Position; }
        public bool IsPaused
        {
            get => SadConsole.Global.CurrentScreen.IsPaused;
            set => SadConsole.Global.CurrentScreen.IsPaused = value;
        }
        //private Dictionary<GameActions, Keys> KeyBindings => Game.Settings.KeyBindings;
        public CSIKeyboardComponent(BasicEntity parent)// : base(isUpdate: true, isKeyboard: true, isDraw: false, isMouse: false)
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

                foreach (Keys key in Game.Settings.MovementKeyBindings.Keys)
                {
                    if (info.IsKeyPressed(key))
                    {
                        moveDirection = Game.Settings.MovementKeyBindings[key];
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
                    if (Global.KeyboardState.IsKeyReleased(action.Value))
                    {
                        TakeAction(action.Key);
                    }
                }
            }

            else
            {
                if (info.IsKeyPressed(Game.Settings.KeyBindings[GameActions.TogglePause]))
                    TogglePause();
                else if (info.IsKeyPressed(Game.Settings.KeyBindings[GameActions.ToggleMenu]))
                    ToggleMenu();
            }
            handled = true;
        }

        public void TakeAction(GameActions key)
        {
            switch (key)
            {
                //opens a magnifying glass?
                case GameActions.DustItemForPrints:
                case GameActions.GetItem:
                case GameActions.LookAtEverythingInSquare: 
                case GameActions.Talk:
                case GameActions.LookAtPerson: OpenCursor(key); break;
                
                case GameActions.RemoveItemFromInventory:
                case GameActions.TakePhotograph:

                case GameActions.ToggleMenu: ToggleMenu(); break;
                case GameActions.TogglePause: TogglePause(); break;
                case GameActions.RefocusOnPlayer: Parent.IsFocused = true; break;
            }
        }

        private void OpenCursor(GameActions key)
        {
            Parent.Components.Add(new MagnifyingGlassComponent(Parent, Parent.Position, key));
        }

        public void ToggleMenu()
        {

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
