using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadConsole.Components;
using System;

namespace Engine.Components
{
    class PhysicalComponent : IGameObjectComponent
    {
        public string Description { get; private set; }
        public int Mass { get; set; }
        public int Volume { get; set; }
        public IGameObject Parent { get; set; }

        public void SetPhysicalDescription(string description)
        {
            Description = description;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
