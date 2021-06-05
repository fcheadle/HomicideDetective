using GoRogue.MapGeneration;
using HomicideDetective.Things;

namespace HomicideDetective.Places
{
    public class Place
    {
        public Region Area { get; set; }
        public Substantive Info { get; set; }
        public MarkingCollection Markings { get; set; }
        
        public Place(Region area, Substantive info, MarkingCollection markings)
        {
            Area = area;
            Info = info;
            Markings = markings;
        }

        public Place(Region area, Substantive info)
        {
            Area = area;
            Info = info;
            Markings = new MarkingCollection();
        }
    }
}