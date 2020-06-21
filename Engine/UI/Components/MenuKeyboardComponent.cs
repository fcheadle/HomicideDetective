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
    public class MenuKeyboardComponent : KeyboardConsoleComponent //as opposed to my own `component` class, which i should really refactor out
    {
        BasicEntity Parent { get; }
        public Coord Position { get => Parent.Position; }
        public bool IsPaused
        {
            get => Global.CurrentScreen.IsPaused;
            set => Global.CurrentScreen.IsPaused = value;
        }
        //private Dictionary<GameActions, Keys> KeyBindings => Game.Settings.KeyBindings;
        public MenuKeyboardComponent(BasicEntity parent)// : base(isUpdate: true, isKeyboard: true, isDraw: false, isMouse: false)
        {
            Parent = parent;
        }

        public override void ProcessKeyboard(SadConsole.Console console, SadConsole.Input.Keyboard info, out bool handled)
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
            if (info.IsKeyPressed(Keys.Escape))
                ToggleMenu();

            if(info.IsKeyPressed(Keys.Enter))
            {
                //get button on this location

            }
            Parent.Position += moveDirection;

            handled = true;
        }

        public void ToggleMenu()
        {
            Game.SwitchUserInterface();
        }
    }
}
