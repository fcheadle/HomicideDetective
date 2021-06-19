using System.Collections.Generic;
using System.Linq;
using SadRogue.Primitives;

namespace HomicideDetective.Places
{
    public class PlaceCollection : List<Place>
    {
        public IEnumerable<Place> GetPlacesContaining(Point point) 
            => this.Where(place => place.Area.Points.Contains(point));

        public bool Contains(Point point) 
            => this.Any(place => place.Area.Points.Contains(point));
    }
}