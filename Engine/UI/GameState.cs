using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.UI
{
    public abstract class GameState : ContainerConsole
    {
        public BasicMap Map { get; set; }
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}
