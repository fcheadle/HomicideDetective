using GoRogue;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Entities.Creatures
{
    public interface ICreatureFactory
    {
        BasicEntity Person(Coord position);
        BasicEntity Player(Coord position);
        BasicEntity Animal(Coord position);
    }
}
