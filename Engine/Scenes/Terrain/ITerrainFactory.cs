using GoRogue;
using SadConsole;

namespace Engine.Scenes.Terrain
{
    //refactor this out
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
