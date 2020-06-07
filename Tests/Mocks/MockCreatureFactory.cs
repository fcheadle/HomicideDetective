using Engine.Entities.Creatures;
using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Tests
{
    internal class MockCreatureFactory : ICreatureFactory
    {
        public BasicEntity Person(Coord position)
        {
            BasicEntity critter = new BasicEntity(Color.White, Color.Black, 2, position, 3, true, true);
            return critter;
        }
        public BasicEntity Player(Coord position)
        {
            BasicEntity critter = new BasicEntity(Color.White, Color.Black, 1, position, 3, true, true);
            return critter;
        }
        public BasicEntity Animal(Coord position)
        {
            BasicEntity critter = new BasicEntity(Color.Gray, Color.Black, 224, position, 3, true, true);
            return critter;
        }
    }
}