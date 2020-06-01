using GoRogue;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Entities.Terrain
{
    public interface ITerrainFactory
    {
        BasicTerrain Generic(Coord position, int glyph);
        BasicTerrain Grass(Coord position, double z = 0);
        BasicTerrain Copy(BasicTerrain source, Coord target);
        BasicTerrain Wall(Coord position);
        BasicTerrain Door(Coord position);
        BasicTerrain Floor(Coord position);
    }
}
