using Engine.Components;
using Engine.Components.Terrain;
using Engine.Extensions;
using Engine.Maps;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Entities
{
    public class ItemFactory
    {
        public BasicEntity Generic(Coord position, string name)
        {
            BasicEntity item = new BasicEntity(Color.White, Color.Transparent, name[0], position, (int)MapLayer.Item, true, true);
            item.AddGoRogueComponent(new PhysicalComponent());
            return item;
        }
    }
}
