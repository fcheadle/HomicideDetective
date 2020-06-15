using Engine.Utilities;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadConsole;
using SadConsole.Components;
using SadConsole.Components.GoRogue;
using SadConsole.Input;
using System;

namespace Engine.Components
{
    public abstract class Component : ComponentBase<BasicEntity>, IConsoleComponent, IGameObjectComponent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; } = 9;
        public bool IsUpdate { get; }
        public bool IsDraw { get; }
        public bool IsMouse { get; }
        public bool IsKeyboard { get; }
        protected Timer timer;
        protected TimeSpan _elapsed;
        public Component(bool isUpdate, bool isKeyboard, bool isDraw, bool isMouse)
        {
            IsUpdate = isUpdate;
            IsKeyboard = isKeyboard;
            IsMouse = isMouse;
            IsDraw = isDraw;
            if (isUpdate)
            {
                timer = new Timer(TimeSpan.FromMilliseconds(Game.TimeIncrement));
                timer.TimerElapsed += (timer, e) => ProcessTimeUnit();
            }
        }

        public abstract void ProcessTimeUnit();

        public abstract string[] GetDetails();
        public virtual void Draw(SadConsole.Console console, TimeSpan delta) { }
        public virtual void OnAdded(SadConsole.Console console){ }
        public virtual void OnRemoved(SadConsole.Console console){ }
        public virtual void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled) { handled = false; }
        public virtual void ProcessMouse(SadConsole.Console console, MouseConsoleState state, out bool handled) { handled = false; }
        public virtual void Update(SadConsole.Console console, TimeSpan delta) { if(Game.Settings.Mode == GameMode.RealTimeWithPause) timer.Update(console, delta); }
    }
}
