using GoRogue;
using SadConsole;
using System;

namespace Engine.Items
{
    public interface IItemFactory
    {
        BasicEntity Generic(Coord position, string name);
        BasicEntity Generic(Coord position, int glyph, string name);
        BasicEntity Functional(Coord position, string name);
        BasicEntity Functional(Coord position, int glyph, string name);
        BasicEntity Furniture(Coord position, string name);
        BasicEntity Furniture(Coord position, int glyph, string name);
        BasicEntity FloorCovering(Coord position, string name);
        BasicEntity FloorCovering(Coord position, int glyph, string name);
        BasicEntity WallCovering(Coord position, string name);
        BasicEntity WallCovering(Coord position, int glyph, string name);
        BasicEntity Container(Coord position, string name, bool locked, bool isWalkable = true, bool isTransparent = true);
        BasicEntity Container(Coord position, int glyph, string name, bool locked, bool isWalkable = true, bool isTransparent = true);
        BasicEntity Trap(Coord position, int glyph, string name, Action<BasicEntity> onTrigger);

    }
}
