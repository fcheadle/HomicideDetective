using GoRogue;
using SadConsole;

namespace Engine.Creatures
{
    public interface ICreatureFactory
    {
        BasicEntity Person(Coord position);
        BasicEntity Player(Coord position);
        BasicEntity Animal(Coord position);
    }
}
