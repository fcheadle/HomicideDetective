using SadConsole;

namespace Engine.States
{
    public abstract class GameState : ContainerConsole
    {
        public BasicMap Map { get; set; }
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}
