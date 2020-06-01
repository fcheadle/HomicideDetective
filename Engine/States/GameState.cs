using Engine.Components;
using Engine.Maps;
using SadConsole;
using System.Collections.Generic;

namespace Engine.States
{
    //todo - kill this, make there be only one state, and the menu is a component
    public abstract class GameState : ContainerConsole
    {
        public SceneMap Map { get; set; }
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}
