using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace HomicideDetective.New.Things
{
    public class Item : RogueLikeEntity, IHaveDetails
    {
        public string Name { get; set; }
        public string Description { get; }
        public Item(Point position, Color foreground, int glyph, string name = "", string description = "", bool walkable = true, bool transparent = true, int layer = 2) : base(position, foreground, Color.Transparent, glyph, walkable, transparent, layer)
        {
            Name = name;
            Description = description;
        }
        public string[] GetDetails()
        {
            throw new System.NotImplementedException();
        }
    }
}