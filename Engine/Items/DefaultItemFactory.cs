using Engine.Components;
using Engine.Scenes;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;
using System;

namespace Engine.Items
{
    public class DefaultItemFactory : IItemFactory
    {
        public BasicEntity Container(Coord position, string name, bool locked, bool isWalkable = true, bool isTransparent = true)
        {
            throw new NotImplementedException();
        }

        public BasicEntity Container(Coord position, int glyph, string name, bool locked, bool isWalkable = true, bool isTransparent = true)
        {
            throw new NotImplementedException();
        }

        public BasicEntity FloorCovering(Coord position, int glyph, string name)
        {
            throw new NotImplementedException();
        }

        public BasicEntity FloorCovering(Coord position, string name)
        {
            throw new NotImplementedException();
        }

        public BasicEntity Functional(Coord position, int glyph, string name)
        {
            throw new NotImplementedException();
        }

        public BasicEntity Functional(Coord position, string name)
        {
            throw new NotImplementedException();
        }

        public BasicEntity Furniture(Coord position, int glyph, string name)
        {
            throw new NotImplementedException();
        }

        public BasicEntity Furniture(Coord position, string name)
        {
            throw new NotImplementedException();
        }

        public BasicEntity Generic(Coord position, string name)
        {
            BasicEntity item = new BasicEntity(Color.White, Color.Transparent, name[0], position, (int)MapLayer.Item, true, true);
            item.Name = name;
            item.Components.Add(new PhysicalComponent(item));
            return item;
        }

        public BasicEntity Generic(Coord position, int glyph, string name)
        {
            BasicEntity item = new BasicEntity(Color.White, Color.Transparent, glyph, position, (int)MapLayer.Item, true, true);
            item.Name = name;
            item.Components.Add(new PhysicalComponent(item));
            return item;
        }

        public BasicEntity Trap(Coord position, int glyph, string name, Action<BasicEntity> onTrigger)
        {
            throw new NotImplementedException();
        }

        public BasicEntity WallCovering(Coord position, int glyph, string name)
        {
            throw new NotImplementedException();
        }

        public BasicEntity WallCovering(Coord position, string name)
        {
            throw new NotImplementedException();
        }
    }
}
