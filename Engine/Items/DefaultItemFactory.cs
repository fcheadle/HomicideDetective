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
        public EntityBase Container(Coord position, string name, bool locked, bool isWalkable = true, bool isTransparent = true)
        {
            throw new NotImplementedException();
        }

        public EntityBase Container(Coord position, int glyph, string name, bool locked, bool isWalkable = true, bool isTransparent = true)
        {
            throw new NotImplementedException();
        }

        public EntityBase FloorCovering(Coord position, int glyph, string name)
        {
            throw new NotImplementedException();
        }

        public EntityBase FloorCovering(Coord position, string name)
        {
            throw new NotImplementedException();
        }

        public EntityBase Functional(Coord position, int glyph, string name)
        {
            throw new NotImplementedException();
        }

        public EntityBase Functional(Coord position, string name)
        {
            throw new NotImplementedException();
        }

        public EntityBase Furniture(Coord position, int glyph, string name)
        {
            throw new NotImplementedException();
        }

        public EntityBase Furniture(Coord position, string name)
        {
            throw new NotImplementedException();
        }

        public EntityBase Generic(Coord position, string name)
        {
            EntityBase item = new EntityBase(Color.White, Color.Transparent, name[0], position, (int)MapLayer.Item, true, true);
            item.Name = name;
            
            return item;
        }

        public EntityBase Generic(Coord position, int glyph, string name)
        {
            EntityBase item = new EntityBase(Color.White, Color.Transparent, glyph, position, (int)MapLayer.Item, true, true);
            item.Name = name;
            
            return item;
        }

        public EntityBase Trap(Coord position, int glyph, string name, Action<BasicEntity> onTrigger)
        {
            throw new NotImplementedException();
        }

        public EntityBase WallCovering(Coord position, int glyph, string name)
        {
            throw new NotImplementedException();
        }

        public EntityBase WallCovering(Coord position, string name)
        {
            throw new NotImplementedException();
        }
    }
}
