using GoRogue.GameFramework.Components;
using SadConsole.Components;
using SadConsole.Components.GoRogue;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Components
{
    class ComponentBase : GameFrameProcessor, IGameObjectComponent, IConsoleComponent
    {
        public int SortOrder => 9;

        public bool IsUpdate => false;

        public bool IsDraw => false;

        public bool IsMouse => false;

        public bool IsKeyboard => false;

        public void Draw(SadConsole.Console console, TimeSpan delta)
        {
            //do nothing
        }

        public void OnAdded(SadConsole.Console console)
        {
            //donothing
        }

        public void OnRemoved(SadConsole.Console console)
        {
            //do nothing
        }

        public override void ProcessGameFrame()
        {
            //do nothing
        }

        public void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled)
        {
            handled = false;
        }

        public void ProcessMouse(SadConsole.Console console, MouseConsoleState state, out bool handled)
        {
            handled = false;
        }

        public void Update(SadConsole.Console console, TimeSpan delta)
        {
            //do nothing
        }
    }
}
