using GoRogue.MapGeneration;
using HomicideDetective.Things;
using SadRogue.Integration;
using SadRogue.Primitives;

namespace HomicideDetective.Places
{
    public class Place
    {
        public Region Area { get; set; }
        public Substantive Info { get; set; }
        public MarkingCollection Markings { get; set; }
        private RogueLikeEntity _entity;
        public Place(Region area, Substantive info, MarkingCollection markings)
        {
            Area = area;
            Info = info;
            Markings = markings;
            InitEntity();
        }

        public Place(Region area, Substantive info)
        {
            Area = area;
            Info = info;
            Markings = new MarkingCollection();
            InitEntity();
        }
        
        private void InitEntity()
        {
            _entity = new RogueLikeEntity(Area.NorthWestCorner, Color.Transparent, Color.Transparent, 0);
            _entity.AllComponents.Add(Info);
            _entity.AllComponents.Add(Markings);
        }
    }
}