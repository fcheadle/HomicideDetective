using GoRogue;

namespace Engine.Creatures
{
    public interface ICreatureFactory
    {
        EntityBase Person(Coord position);
        EntityBase Player(Coord position);
        EntityBase Animal(Coord position);
    }
}
