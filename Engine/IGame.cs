using Engine.Entities.Creatures;
using Engine.Entities.Items;
using Engine.Entities.Terrain;
using Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public interface IGame
    {
        ISettings Settings { get; }
        ITerrainFactory TerrainFactory{ get; }
        IItemFactory ItemFactory { get; }
        ICreatureFactory CreatureFactory { get; }
        void Init();
        void Start();
        void Stop();
        void Update(Microsoft.Xna.Framework.GameTime time);
    }
}
