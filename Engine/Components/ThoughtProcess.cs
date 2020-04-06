using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadConsole.Components;
using System;

namespace Engine.Components
{
    class ThoughtProcess : LogicConsoleComponent, IGameObjectComponent
    {
        public IGameObject Parent { get; set; }

        public override void Draw(SadConsole.Console console, TimeSpan delta)
        {
            //no need to draw
        }

        public override void Update(SadConsole.Console console, TimeSpan delta)
        {
            //can't think yet
        }
    }
}
