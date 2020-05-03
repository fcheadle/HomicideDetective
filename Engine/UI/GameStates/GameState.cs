using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.UI
{
    abstract class GameState
    {
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}
