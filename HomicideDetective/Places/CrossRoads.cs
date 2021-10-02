using HomicideDetective.Places.Generation;

namespace HomicideDetective.Places
{
    public class CrossRoads
    {
        public string Name => $"{(int)Street}00 block {(RoadNames)Road} street";
        public MapType Type { get; set; }
        public RoadNames Road { get; set; }
        public RoadNumbers Street { get; set; }
        
        public CrossRoads(MapType type, RoadNames road, RoadNumbers street)
        {
            Type = type;
            Road = road;
            Street = street;
        }
    }
}