using GoRogue;
using SadConsole;
using System;

namespace Engine.Items
{
    public interface IItemFactory
    {
        EntityBase Generic(Coord position, string name);
        EntityBase Generic(Coord position, int glyph, string name);
        EntityBase Functional(Coord position, string name);
        EntityBase Functional(Coord position, int glyph, string name);
        EntityBase Furniture(Coord position, string name);
        EntityBase Furniture(Coord position, int glyph, string name);
        EntityBase FloorCovering(Coord position, string name);
        EntityBase FloorCovering(Coord position, int glyph, string name);
        EntityBase WallCovering(Coord position, string name);
        EntityBase WallCovering(Coord position, int glyph, string name);
        EntityBase Container(Coord position, string name, bool locked, bool isWalkable = true, bool isTransparent = true);
        EntityBase Container(Coord position, int glyph, string name, bool locked, bool isWalkable = true, bool isTransparent = true);
        EntityBase Trap(Coord position, int glyph, string name, Action<BasicEntity> onTrigger);
    }
}
