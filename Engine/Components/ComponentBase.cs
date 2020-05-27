using GoRogue.GameFramework.Components;
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
        public ComponentBase(bool isUpdate, bool isKeyboard, bool isDraw, bool isMouse)
        {
            IsUpdate = isUpdate;
            IsKeyboard = isKeyboard;
            IsMouse = isMouse;
            IsDraw = isDraw;
        }

        //We need to define these and have them throw exceptions, becuse if we leave them abstract, then they require declaration even in classes where they aren't needed.
        public virtual void Draw(SadConsole.Console console, TimeSpan delta) => throw new NotImplementedException();
        public virtual void OnAdded(SadConsole.Console console) => throw new NotImplementedException();
        public virtual void OnRemoved(SadConsole.Console console) => throw new NotImplementedException();
        public abstract string[] GetDetails();
        public abstract override void ProcessGameFrame();
        public virtual void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled) => throw new NotImplementedException();
        public virtual void ProcessMouse(SadConsole.Console console, MouseConsoleState state, out bool handled) => throw new NotImplementedException();
        public virtual void Update(SadConsole.Console console, TimeSpan delta) => throw new NotImplementedException();
    }
}
