using GoRogue.GameFramework.Components;
using SadConsole;
using SadConsole.Components;
using SadConsole.Components.GoRogue;
using SadConsole.Input;
using System;

namespace Engine.Components
{
    public abstract class ComponentBase : GameFrameProcessor, IGameObjectComponent, IConsoleComponent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; } = 9;
        public bool IsUpdate { get; } = false;
        public bool IsDraw { get; } = false;
        public bool IsMouse { get; } = false;
        public bool IsKeyboard { get; } = false;
        protected Timer timer;
        public ComponentBase(bool isUpdate, bool isKeyboard, bool isDraw, bool isMouse)
        {
            IsUpdate = isUpdate;
            IsKeyboard = isKeyboard;
            IsMouse = isMouse;
            IsDraw = isDraw;
            if (isUpdate)
            {
                timer = new Timer(TimeSpan.FromMilliseconds(100));
                timer.TimerElapsed += (timer, e) => ProcessGameFrame();
            }
        }

        public abstract string[] GetDetails();
        public abstract override void ProcessGameFrame();
        public virtual void Draw(SadConsole.Console console, TimeSpan delta) { }
        public virtual void OnAdded(SadConsole.Console console){ }
        public virtual void OnRemoved(SadConsole.Console console){ }
        public virtual void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled) { handled = false; }
        public virtual void ProcessMouse(SadConsole.Console console, MouseConsoleState state, out bool handled) { handled = false; }
        public virtual void Update(SadConsole.Console console, TimeSpan delta) { timer.Update(console, delta); }
    }
}
