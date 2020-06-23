using GoRogue;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Components;
using SadConsole.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Engine.UI.Components
{
    public class MenuKeyboardComponent : KeyboardConsoleComponent //as opposed to my own `component` class, which i should really refactor out
    {
        BasicEntity Parent { get; }
        public Coord Position { get => Parent.Position; }
        private int _buttonIndex = 0;
        private ReadOnlyCollection<ControlBase> _controls;
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
            _controls = Game.Menu.ActivePanels.Peek().Controls;
            if (info.IsKeyPressed(Keys.Up))
                MoveUp();

            if (info.IsKeyPressed(Keys.Down))
                MoveDown();

            if (info.IsKeyPressed(Keys.Left))
                HideParent();
            if (info.IsKeyPressed(Keys.Escape))
                HideParent();

            if (info.IsKeyPressed(Keys.Right))
                Select();
            if (info.IsKeyPressed(Keys.Enter))
                Select();

            handled = true;
        }

        public void Select()
        {
            MenuPanel active = Game.Menu.ActivePanels.Peek();
            active.SelectControl(_buttonIndex);
        }

        public void HideParent()
        {
            Game.Menu.ActivePanels.Pop();
        }

        public void MoveDown()
        {
            _buttonIndex++;
            if (_buttonIndex >= _controls.Count)
                _buttonIndex = 0;
        }

        public void MoveUp()
        {
            _buttonIndex--;
            if (_buttonIndex < 0)
                _buttonIndex = _controls.Count - 1;
        }
    }
}
