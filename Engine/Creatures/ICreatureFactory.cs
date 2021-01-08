using GoRogue;

namespace Engine.Creatures
{
    //can probably do without this class
    public interface ICreatureFactory
    {
        EntityBase Person(Coord position);
        EntityBase Player(Coord position);
        EntityBase Animal(Coord position);
    }
}
